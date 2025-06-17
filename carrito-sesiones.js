const API_CARRITO = 'https://localhost:7279/api/CarritoCompras';
const API_LIBROS = 'https://localhost:7119/api/LibroMaterial';

let librosDisponibles = [];
let totalCarrito = 0;

// Carga los libros disponibles al inicio
function fetchLibrosDisponibles() {
    fetch(API_LIBROS)
        .then(response => {
            if (!response.ok) throw new Error('Error al obtener los libros disponibles.');
            return response.json();
        })
        .then(data => {
            librosDisponibles = (data || []).map(libro => ({
                ...libro,
                precio: 1200 // Asigna un precio fijo de 1200 pesos a todos los libros
            }));
            console.log('Libros disponibles:', librosDisponibles); // Depuración
        })
        .catch(error => {
            console.error('Error al cargar los libros disponibles:', error);
        });
}

// Busca el carrito por ID
document.getElementById('buscar-carrito-btn').addEventListener('click', () => {
    const carritoId = document.getElementById('carrito-id').value.trim();
    if (!carritoId) {
        alert('Por favor, ingresa un ID de carrito.');
        return;
    }

    fetch(`${API_CARRITO}/${carritoId}`)
        .then(response => {
            if (!response.ok) throw new Error('No se pudo obtener el carrito.');
            return response.json();
        })
        .then(data => {
            console.log('Carrito obtenido:', data); // Depuración
            renderCarritoDetalle(data);
        })
        .catch(error => {
            console.error('Error al obtener el carrito:', error);
            document.getElementById('carrito-libros').innerHTML = '<p class="text-center text-danger">Error al cargar el carrito.</p>';
        });
});

// Renderiza los detalles del carrito
function renderCarritoDetalle(carrito) {
    const carritoLibros = document.getElementById('carrito-libros');
    carritoLibros.innerHTML = '';
    totalCarrito = 0;

    if (!carrito || !carrito.listaProductos || !carrito.listaProductos.length) {
        carritoLibros.innerHTML = '<p class="text-center">No hay libros en este carrito.</p>';
        document.getElementById('total-price').textContent = '0';
        return;
    }

    carrito.listaProductos.forEach(producto => {
        const libroId = producto.libroId;
        const libro = librosDisponibles.find(l => String(l.libreriaMaterialId) === String(libroId));

        const precio = libro?.precio ?? 1200; // Usa el precio fijo si no se encuentra el libro
        totalCarrito += precio;

        const item = document.createElement('div');
        item.className = 'carrito-item';
        item.innerHTML = `
            <span>${libro?.titulo ?? `ID: ${libroId}`}</span>
            <span>Precio: $${precio}</span>
        `;
        carritoLibros.appendChild(item);
    });

    // Actualiza el total del carrito
    document.getElementById('total-price').textContent = totalCarrito;
}

// Carga los libros disponibles al cargar la página
fetchLibrosDisponibles();
