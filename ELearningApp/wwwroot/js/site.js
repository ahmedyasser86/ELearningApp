// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', function () {
    var deleteButtons = document.querySelectorAll('.btn-danger');

    deleteButtons.forEach(function (btn) {
        btn.addEventListener('click', function (event) {
            if (!confirm('Are you sure you want to delete this course?')) {
                event.preventDefault();
            }
        });
    });
});
