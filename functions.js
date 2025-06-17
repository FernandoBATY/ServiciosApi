let carritoTemporal = [];

// Renderiza la lista de libros seleccionados en el carrito
function renderCarrito() {
    const carritoContainer = document.getElementById('carrito-lista');
    carritoContainer.innerHTML = '';

    if (!carritoTemporal.length) {
        carritoContainer.innerHTML = '<p class="text-center">No hay libros en el carrito.</p>';
        return;
    }

    carritoTemporal.forEach((libro, idx) => {
        const item = document.createElement('div');
        item.className = 'carrito-item';
        item.innerHTML = `
            <span>${libro.titulo ?? 'Sin título'}</span>
            <button class="remove-btn" data-index="${idx}">❌</button>
        `;
        carritoContainer.appendChild(item);
    });

    // Agrega eventos para eliminar libros del carrito
    const removeBtns = carritoContainer.querySelectorAll('.remove-btn');
    removeBtns.forEach(btn => {
        btn.addEventListener('click', function () {
            const index = parseInt(this.getAttribute('data-index'), 10);
            carritoTemporal.splice(index, 1);
            renderCarrito();
        });
    });
}

// Evento para agregar libros al carrito temporal
function agregarAlCarrito(libro) {
    carritoTemporal.push(libro);
    renderCarrito();
}

// Evento para realizar el POST de todos los libros seleccionados
function enviarCarrito() {
    if (!carritoTemporal.length) {
        alert('El carrito está vacío.');
        return;
    }

    fetch(API_CARRITO, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            fechaCreacionSesion: new Date().toISOString(),
            productoLista: carritoTemporal.map(libro => libro.libreriaMaterialId)
        })
    })
    .then(response => {
        if (!response.ok) throw new Error('No se pudo completar la compra');
        return response.json();
    })
    .then(() => {
        alert('Compra realizada con éxito');
        carritoTemporal = [];
        renderCarrito();
    })
    .catch(() => {
        alert('Error al realizar la compra');
    });
}

// Modifica el renderizado del carrusel para usar la nueva funcionalidad
function renderCarousel() {
    const carousel = document.getElementById('card-3d');
    carousel.innerHTML = '';

    if (!libros.length) {
        carousel.innerHTML = '<div class="text-center"><p>No hay libros disponibles.</p></div>';
        return;
    }

    // Muestra hasta 10 libros en el círculo 3D
    const maxCards = 10;
    for (let i = 0; i < Math.min(libros.length, maxCards); i++) {
        const libro = libros[i];
        // Busca el autor correspondiente por GUID
        let autor = null;
        if (window.autores && libro.autorLibro) {
            autor = window.autores.find(a =>
                (a.autorLibroGuid || a.AutorLibroGuid) &&
                (a.autorLibroGuid || a.AutorLibroGuid).toLowerCase() === String(libro.autorLibro).toLowerCase()
            );
        }
        const autorNombre = autor
            ? `${autor.nombre ?? autor.Nombre ?? ''} ${autor.apellido ?? autor.Apellido ?? ''}`.trim()
            : 'Autor desconocido';
        const autorGuid = autor ? (autor.autorLibroGuid ?? autor.AutorLibroGuid) : libro.autorLibro;

        const card = document.createElement('div');
        card.className = 'book-card';
        card.innerHTML = `
            <div class="book-card__cover">
                <img src="${libro.imagenUrl ?? 'https://images.unsplash.com/photo-1634715841611-67741dc71459?q=80&w=2031&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D'}" alt="${libro.titulo ?? 'Sin título'}">
            </div>
            <div class="book-card__info">
                <h3 class="book-card__title">${libro.titulo ?? 'Sin título'}</h3>
                <p class="book-card__date">${libro.fechaPublicacion ? 'Publicado: ' + libro.fechaPublicacion : 'Sin fecha de publicación'}</p>
                <p class="book-card__author">
                    <span class="autor-link" data-autorguid="${autorGuid ?? ''}" style="color:#4f8cff;cursor:pointer;text-decoration:underline;">
                        ${autorNombre}
                    </span>
                </p>
            </div>
            <div class="book-card__footer">
                <span class="book-card__price">$${libro.precio ?? 1200}</span>
                <button class="book-card__btn" title="Agregar al carrito">🛒</button>
            </div>
        `;
        carousel.appendChild(card);

        // Evento para agregar al carrito temporal
        const btn = card.querySelector('.book-card__btn');
        btn.addEventListener('click', () => agregarAlCarrito(libro));
    }

    // Eventos para pausar animación
    const card3d = document.getElementById('card-3d');
    const cards = card3d.querySelectorAll('.book-card');
    cards.forEach(card => {
        card.addEventListener('mouseenter', () => {
            card3d.style.animationPlayState = 'paused';
            cards.forEach(c => c.style.animationPlayState = 'paused');
        });
        card.addEventListener('mouseleave', () => {
            card3d.style.animationPlayState = '';
            cards.forEach(c => c.style.animationPlayState = '');
        });
    });

    // Evento para mostrar modal al hacer click en el autor
    const autorLinks = carousel.querySelectorAll('.autor-link');
    autorLinks.forEach(link => {
        link.addEventListener('click', function () {
            const guid = this.getAttribute('data-autorguid');
            if (guid) {
                fetchAutorDetalle(guid);
            }
        });
    });
}


function showPrev() {}
function showNext() {}
function setupCarouselEvents() {}
function setupCarouselEvents() {}
