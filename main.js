//Esta es el api de get para obtener los libros que estan en la base de datos de libro 
const API_URL = 'https://localhost:7119/api/LibroMaterial';

let libros = [];
let currentIndex = 0;
const visibleCards = 1; // Número de cartas visibles en el carrusel

document.addEventListener('DOMContentLoaded', () => {
    fetch(API_URL)
        .then(response => {
            if (!response.ok) throw new Error('Error al obtener libros');
            return response.json();
        })
        .then(data => {
            libros = data || [];
            currentIndex = 0;
            renderCarousel(); // Esta función estará en functions.js
            setupCarouselEvents(); // Esta función estará en functions.js
        })
        .catch(error => {
            document.getElementById('card-carousel').innerHTML = '<p class="text-danger text-center">Error al cargar los libros.</p>';
            console.error(error);
        });
});

