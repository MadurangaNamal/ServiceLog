(function () {
    const card = document.getElementById('repairRecordsCard');
    if (!card) return;

    const pageSize = Number.parseInt(card.dataset.pageSize, 5) || 5;
    const rows = [...card.querySelectorAll('[data-repair-row]')];
    const totalPages = Math.ceil(rows.length / pageSize);
    if (totalPages <= 1) return;

    let currentPage = 1;
    const infoEl = card.querySelector('[data-pagination-info]');
    const pageEl = card.querySelector('[data-pagination-page]');
    const prevItem = card.querySelector('[data-page-prev]')?.closest('.page-item');
    const nextItem = card.querySelector('[data-page-next]')?.closest('.page-item');

    function render() {
        const start = (currentPage - 1) * pageSize;
        const end = Math.min(start + pageSize, rows.length);

        rows.forEach((row, index) => {
            row.hidden = index < start || index >= end;
        });

        if (infoEl) {
            infoEl.textContent = `Showing ${start + 1}\u2013${end} of ${rows.length}`;
        }

        if (pageEl) {
            pageEl.textContent = `Page ${currentPage} of ${totalPages}`;
        }

        prevItem?.classList.toggle('disabled', currentPage === 1);
        nextItem?.classList.toggle('disabled', currentPage === totalPages);
    }

    card.querySelector('[data-page-prev]')?.addEventListener('click', () => {
        if (currentPage > 1) {
            currentPage--;
            render();
        }
    });

    card.querySelector('[data-page-next]')?.addEventListener('click', () => {
        if (currentPage < totalPages) {
            currentPage++;
            render();
        }
    });

    render();
})();