 
/////
// Mapping object for page-specific initialization logic
const pageInitializers = {
    "/Invoice/Create": () => {
        if (window.initializeInvoiceSearch) window.initializeInvoiceSearch();
        if (window.LoadInvoiceItems) window.LoadInvoiceItems();
        if (window.initUserForm) window.initUserForm();
    },
    "/User/Index": [
        () => { if (window.loadCustomers) window.loadCustomers(); },
        () => { document.getElementById("checkbox-bulk-customers-select").addEventListener("change", toggleSelectAll); }
    ],
    "/Invoice/Index": [
        () => { window.loadInvoices();},
        () => { document.getElementById("checkbox-bulk-customers-select").addEventListener("change", toggleSelectAll); }
    ],
    "/Product/Products": [
        () => {
            window.loadInvoices();
            //window.renderProductList();
        },
        () => { document.getElementById("checkbox-bulk-customers-select").addEventListener("change", toggleSelectAll); }
    ],
    //"/Customer/Create": [
    //    () => {
    //        if (window.initUserForm) window.initUserForm();
    
    //    }
    //],
    "/User/Create": [
        () => {
            if (window.initUserForm) window.initUserForm();

        }
    ],
    
    "/InvoicType/index": (isAjax) => {
        if (isAjax && window.loadTable) window.loadTable();
    } 
    // Add more URLs and their initialization functions here as needed
};

// Function to initialize page-specific logic based on URL
function initializePage(url, isAjax = false) {
    const parsedUrl = new URL(url, window.location.origin);
    const path = parsedUrl.pathname; // Get the pathname (e.g., /Invoice/Create)
    console.log('Initializing page for URL:', url);
    console.log('Initializing page for path:', path);
    const initializers = pageInitializers[path];
    //const initializers = pageInitializers[url];
    if (initializers) {
        if (Array.isArray(initializers)) {
            // If it's an array, call each function with isAjax
            initializers.forEach(init => init(isAjax));
        } else {
            // If it's a single function, call it with isAjax
            initializers(isAjax);
        }
        

    } else {
        console.warn(`No initializer found for URL: ${url}`);
        console.warn(`No initializer found for URL: ${path}`);
    }
}

// Function to load page content via AJAX and update the URL
function loadPage(url) {
    $.ajax({
        url: url,
        type: 'GET',
        success: function (data) {
            $('#MainContainer').html(data); // Update the content container
            window.history.pushState({}, '', url); // Update the browser URL
            initializePage(url, true); // Initialize with isAjax=true
        },
        error: function () {
            alert('Error loading page.');
        }
    });
}

// Handle browser back/forward navigation
window.onpopstate = function () {
  //  loadPage(location.pathname);
    loadPage(window.location.href);
};

// Initialize the page on initial load or full reload
document.addEventListener("DOMContentLoaded", function () {
    //initializePage(location.pathname); // Initialize with isAjax=false
    initializePage(window.location.href); // Pass full URL
});