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
        const card = document.createElement('div');
        card.className = 'book-card';
        // El footer debe estar FUERA de book-card__info para que flexbox lo posicione abajo
        card.innerHTML = `
            <div class="book-card__cover">
                <img src="${libro.imagenUrl ?? 'https://images.unsplash.com/photo-1634715841611-67741dc71459?q=80&w=2031&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D'}" alt="${libro.titulo ?? 'Sin título'}">
            </div>
            <div class="book-card__info">
                <h3 class="book-card__title">${libro.titulo ?? 'Sin título'}</h3>
                <p class="book-card__date">${libro.fechaPublicacion ? 'Publicado: ' + libro.fechaPublicacion : 'Sin fecha de publicación'}</p>
                <p class="book-card__author">${libro.autorNombre ? 'Autor: ' + libro.autorNombre : 'Autor desconocido'}</p>
            </div>
            <div class="book-card__footer">
                <span class="book-card__price">$${libro.precio ?? '--'}</span>
                <button class="book-card__btn" title="Agregar al carrito">🛒</button>
            </div>
        `;
        carousel.appendChild(card);
    }
}

// Las funciones de navegación ya no son necesarias en el modo 3D
function showPrev() {}
function showNext() {}
function setupCarouselEvents() {}
