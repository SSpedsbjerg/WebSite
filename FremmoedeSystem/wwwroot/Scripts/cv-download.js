function downloadCVAsPDF() {
    const pdfUrl = "/Other/CV.pdf";
    const link = document.createElement("a");
    link.href = pdfUrl;
    link.download = "SimonSpedsbjergCV.pdf";
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

window.downloadCVAsPDF = downloadCVAsPDF;

function downloadMastersPDF() {
    const pdfUrl = "/Other/Root_Causing_and_Event_Identification_Through_Simon_Spedsbjerg.pdf"
    const link = document.createElement("a");
    link.href = pdfUrl;
    link.download = "Root_Causing_and_Event_Identification_Through_Simon_Spedsbjerg.pdf";
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

window.downloadMastersPDF = downloadMastersPDF;

window.observeFlyIn = () => {
    const button = document.querySelector('.CV-download-button.fly-in');
    if (!button) return;

    const observer = new IntersectionObserver(entries => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                button.classList.add('visible');
                observer.unobserve(entry.target);
            }
        });
    }, {
        threshold: 0.5
    });

    observer.observe(document.getElementById('cv-download-section'));
};
