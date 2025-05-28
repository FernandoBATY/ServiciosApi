//Esta es el api de get para obtener los libros que estan en la base datos, relacionado con el proyecto de Libro 
const API_URL = 'https://localhost:7119/api/LibroMaterial';

fetch(API_URL)
    .then(response => {
        if (!response.ok) throw new Error('Error al obtener libros');
        return response.json();
    })
    .then(data => {
        const carouselInner = document.getElementById('carouselInner');
        carouselInner.innerHTML = '';

        if (!data || data.length === 0) {
            carouselInner.innerHTML = '<div class="text-center"><p>No hay libros disponibles.</p></div>';
            return;
        }

        data.forEach((libro, index) => {
            const item = document.createElement('div');
            item.className = 'carousel-item' + (index === 0 ? ' active' : '');
            item.innerHTML = `
        <div class="d-flex justify-content-center align-items-center slide-item">
          <div class="text-center">                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
            <h3>${libro.titulo ?? 'Sin t�tulo'}</h3>
            <p>${libro.fechaPublicacion ? 'Publicado: ' + libro.fechaPublicacion : 'Sin fecha de publicaci�n'}</p>
          </div>
        </div>
      `;
            carouselInner.appendChild(item);
        });
    })
    .catch(error => {
        document.getElementById('carouselInner').innerHTML = '<p class="text-danger text-center">Error al cargar los libros.</p>';
        console.error(error);
    });
    
