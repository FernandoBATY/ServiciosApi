//Esta es el api de get para obtener los libros que estan en la base de datos de libro
//No mover tania y arce
const API_URL = 'https://localhost:7119/api/LibroMaterial';
const API_AUTOR_URL = 'https://localhost:7205/api/Autor';
const API_AUTOR_GRADO_URL = 'https://localhost:7205/api/Autor/grado';

let libros = [];
let autores = [];
let gradosAcademicos = [];
let currentIndex = 0;
const visibleCards = 3; // Número de cartas visibles en el carrusel

// Obtener libros
function fetchLibros() {
    fetch(API_URL)
        .then(response => {
            if (!response.ok) throw new Error('Error al obtener libros');
            return response.json();
        })
        .then(data => {
            libros = data || [];
            currentIndex = 0;
            renderCarousel(); // Esta función estará en functions.js asi que no borrar cofa
            setupCarouselEvents(); // Esta función estará en functions.js y esta tampoco
        })
        .catch(error => {
            document.getElementById('card-carousel') && (document.getElementById('card-carousel').innerHTML = '<p class="text-danger text-center">Error al cargar los libros.</p>');
            console.error(error);
        });
}

// Obtener autores
function fetchAutores() {
    fetch(API_AUTOR_URL)
        .then(response => {
            if (!response.ok) throw new Error('Error al obtener autores');
            return response.json();
        })
        .then(data => {
            autores = data || [];
            window.autores = autores; // <-- Añade esto
            console.log('Autores:', autores);
            // Vuelve a renderizar el carrusel si ya hay libros
            if (libros.length) renderCarousel();
        })
        .catch(error => {
            console.error('Error al cargar autores:', error);
        });
}

// Obtener todos los grados académicos y emparejarlos por índice con los autores
function fetchGradosAcademicos() {
    return fetch(API_AUTOR_GRADO_URL)
        .then(response => {
            if (!response.ok) throw new Error('Error al obtener grados académicos');
            return response.json();
        })
        .then(data => {
            gradosAcademicos = data || [];
        })
        .catch(() => {
            gradosAcademicos = [];
        });
}

// Obtener detalle de autor por GUID y mostrar en modal
function fetchAutorDetalle(autorGuid) {
    // Busca primero en window.autores para evitar error 404 si el API no lo encuentra
    const autorLocal = window.autores?.find(a =>
        (a.autorLibroGuid || a.AutorLibroGuid)?.toLowerCase() === String(autorGuid).toLowerCase()
    );
    if (autorLocal) {
        showAutorModal(autorLocal);
        return;
    }

    // Si no está en la lista local, intenta el fetch (por si el API soporta GET /api/Autor/{guid})
    fetch(`${API_AUTOR_URL}/${autorGuid}`)
        .then(response => {
            if (!response.ok) throw new Error('Error al obtener detalle del autor');
            return response.json();
        })
        .then(data => {
            showAutorModal(data);
        })
        .catch(error => {
            showAutorModal({ Nombre: 'Error', Apellido: '', FechaNacimiento: '', AutorLibroGuid: autorGuid, error: true });
        });
}

// Mostrar modal con la información del autor y su grado académico emparejado por índice
function showAutorModal(autor) {
    let modal = document.getElementById('autor-modal');
    if (!modal) {
        modal = document.createElement('div');
        modal.id = 'autor-modal';
        modal.className = 'autor-modal';
        modal.innerHTML = `
            <div class="autor-modal-content" id="autor-modal-content">
                <span class="autor-modal-close" id="autor-modal-close">&times;</span>
                <div id="autor-modal-body"></div>
            </div>
        `;
        document.body.appendChild(modal);
        document.getElementById('autor-modal-close').onclick = hideAutorModal;
        modal.onclick = function(e) {
            if (e.target === modal) hideAutorModal();
        };
    }
    const body = document.getElementById('autor-modal-body');
    if (autor.error) {
        body.innerHTML = `<p>No se pudo cargar la información del autor.</p>`;
        modal.style.display = 'flex';
        return;
    }

    // Encuentra el índice del autor en la lista de autores
    const idx = window.autores?.findIndex(a =>
        (a.autorLibroGuid || a.AutorLibroGuid)?.toLowerCase() === String(autor.autorLibroGuid ?? autor.AutorLibroGuid).toLowerCase()
    );
    // Empareja el grado académico por índice
    const grado = (idx !== undefined && idx >= 0 && idx < gradosAcademicos.length) ? gradosAcademicos[idx] : null;

    body.innerHTML = `
        <h3>${autor.nombre ?? autor.Nombre ?? ''} ${autor.apellido ?? autor.Apellido ?? ''}</h3>
        <p><strong>Fecha de nacimiento:</strong> ${autor.fechaNacimiento ? new Date(autor.fechaNacimiento).toLocaleDateString() : 'No disponible'}</p>
        <p><strong>GUID:</strong> ${autor.autorLibroGuid ?? autor.AutorLibroGuid ?? ''}</p>
        <div id="autor-grados">
            ${
                grado
                ? `<h4>Grado Académico</h4>
                    <ul>
                        <li>
                            <strong>${grado.nombre ?? grado.Nombre ?? ''}</strong> - 
                            ${grado.centroAcademico ?? grado.CentroAcademico ?? ''} 
                            (${grado.fechaGrado ? new Date(grado.fechaGrado).toLocaleDateString() : 'Sin fecha'})
                        </li>
                    </ul>`
                : '<p>No hay grados académicos registrados.</p>'
            }
        </div>
    `;
    modal.style.display = 'flex';
}

// Ocultar modal
function hideAutorModal() {
    const modal = document.getElementById('autor-modal');
    if (modal) modal.style.display = 'none';
}

document.addEventListener('DOMContentLoaded', () => {
    Promise.all([fetchLibros(), fetchAutores(), fetchGradosAcademicos()]);
});

