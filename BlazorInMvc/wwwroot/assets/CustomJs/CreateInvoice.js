//function InvoiceProductSearch() {
//    const searchInput = document.getElementById('chooseProduct');
//    const searchDropdown = document.getElementById('searchDropdown');
//    console.log(searchInput, searchDropdown);
//    searchInput.addEventListener('input', function () {
//        if (this.value.length > 0) {
//            searchDropdown.style.display = 'block';
//            // Add logic here to populate dropdown dynamically based on input
//        } else {
//            searchDropdown.style.display = 'none';
//        }
//    });

//    document.addEventListener('click', function (e) {
//        if (!searchInput.contains(e.target) && !searchDropdown.contains(e.target)) {
//            searchDropdown.style.display = 'none';
//        }
//    });
//}

//window.InvoiceProductSearch = InvoiceProductSearch;
//function InvoiceProductSearch() {
//    const searchInput = document.getElementById('chooseProduct');
//    const searchDropdown = document.getElementById('searchDropdown');
//    const dropdownList = searchDropdown.querySelector('.list');
//    const noResultsMessage = searchDropdown.querySelector('.text-center p');
//    let allProducts = []; // Store all products locally
//    let isInitialized = false; // Track if products are fetched

//    // Validate DOM elements
//    if (!searchInput) console.error('searchInput not found');
//    if (!searchDropdown) console.error('searchDropdown not found');
//    if (!dropdownList) console.error('dropdownList not found');
//    if (!noResultsMessage) console.error('noResultsMessage not found');
//    if (!searchInput || !searchDropdown || !dropdownList || !noResultsMessage) {
//        console.error('Required DOM elements missing. Check HTML IDs and DOM load timing.');
//        return;
//    }

//    console.log('InvoiceProductSearch initialized');

//    // Fetch all products from the API
//    async function fetchAllProducts() {
//        try {
//            const response = await fetch('/api/GetProducts');
//            const data = await response.json();
//            if (data.isSuccess) {
//                allProducts = data.product_list;
//            } else {
//                allProducts = [];
//            }
//            console.log('All products fetched:', allProducts);
//            isInitialized = true;
//        } catch (error) {
//            console.error('Error fetching products:', error);
//            allProducts = [];
//            isInitialized = true;
//        }
//    }

//    // Filter products locally based on query
//    function filterProducts(query) {
//        if (!query) return [];
//        query = query.toLowerCase().trim();
//        console.log('[DEBUG] Filtering products with query:', query); // Debug Point 2
//        const filtered = allProducts.filter(product => {
//            const nameMatch = product.prodName?.toLowerCase().includes(query) || false;
//            const skuMatch = product.sku?.toLowerCase().includes(query) || false;
//            const codeMatch = product.productCode?.toLowerCase().includes(query) || false;
//            return nameMatch || skuMatch || codeMatch;
//        });
//        console.log('[DEBUG] Filtered products:', filtered); // Debug Point 3
//        return filtered;
//    }

//    // Populate dropdown with filtered products
//    function populateDropdown(products) {
//        dropdownList.innerHTML = '';
//        console.log('[DEBUG] Populating dropdown with:', products); // Debug Point 4
//        if (products.length > 0) {
//            noResultsMessage.classList.add('d-none');
//            products.forEach(product => {
//                const item = document.createElement('a');
//                item.className = 'dropdown-item px-x1 py-2';
//                item.href = '#';
//                item.innerHTML = `
//                    <div class="d-flex align-items-center">
//                        <div class="avatar avatar-l me-2">
//                            <img class="rounded-circle" src="${product.imageUrl || '../../assets/img/default-product.jpg'}" alt="${product.prodName}" />
//                        </div>
//                        <div class="flex-1">
//                            <h6 class="mb-0 title">${product.prodName}</h6>
//                            <p class="fs-11 mb-0 d-flex">${product.sku || product.productCode || 'N/A'}</p>
//                        </div>
//                    </div>
//                `;
//                item.addEventListener('click', () => {
//                    searchInput.value = product.prodName;
//                    searchDropdown.style.display = 'none';
//                });
//                dropdownList.appendChild(item);
//            });
//        } else {
//            noResultsMessage.classList.remove('d-none');
//        }
//    }

//    // Handle search input
//    function handleSearch() {
//        console.log('[DEBUG] handleSearch triggered, isInitialized:', isInitialized); // Debug Point 1
//        if (!isInitialized) {
//            console.log('Products not yet loaded, waiting...');
//            return;
//        }
//        const query = searchInput.value;
//        console.log('[DEBUG] Search query:', query); // Debug Point 1.5
//        if (query.length > 0) {
//            searchDropdown.style.display = 'block';
//            const filteredProducts = filterProducts(query);
//            populateDropdown(filteredProducts);
//        } else {
//            searchDropdown.style.display = 'none';
//            dropdownList.innerHTML = '';
//            noResultsMessage.classList.add('d-none');
//        }
//    }

//    // Initialize the function
//    async function init() {
//        console.log('Starting initialization...');
//        await fetchAllProducts();

//        console.log('Attaching input event listener to searchInput');
//        searchInput.addEventListener('input', () => {
//            console.log('[DEBUG] Input event fired'); // Debug Point 0
//            handleSearch();
//        });

//        console.log('Attaching click event listener to document');
//        document.addEventListener('click', (e) => {
//            if (!searchInput.contains(e.target) && !searchDropdown.contains(e.target)) {
//                searchDropdown.style.display = 'none';
//            }
//        });
//    }

//    // Start initialization
//    init();
//}

//document.addEventListener('DOMContentLoaded', () => {
//    console.log('DOM fully loaded, calling InvoiceProductSearch');
//    InvoiceProductSearch();
//});
//window.InvoiceProductSearch = InvoiceProductSearch;


// Invoice Search Functionality

