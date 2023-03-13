class GDPR {

    constructor() {
        this.showStatus();
        this.showContent();
        this.bindEvents();

        if (this.cookieStatus() !== 'accept' && this.cookieStatus() !== 'reject') {
            this.showGDPR();
        }

        this.saveMetaData();
    }

    bindEvents() {
        try {
            let buttonAccept = document.querySelector('.gdpr-consent__button--accept');
            buttonAccept.addEventListener('click', () => {
                this.cookieStatus('accept');
                this.showStatus();
                this.showContent();
                this.hideGDPR();
            });



            let buttonReject = document.querySelector('.gdpr-consent__button--reject');
            buttonReject.addEventListener('click', () => {
                this.cookieStatus('reject');
                this.showStatus();
                this.hideGDPR();

            });
        }
        catch (e) {

        }
    }


    showContent() {
        this.resetContent();
        const status = this.cookieStatus() == null ? 'not-chosen' : this.cookieStatus();
        const element = document?.querySelectorAll(`.content-gdpr-${status}`);

        for (const e of element) {
            e.classList.add('show');
        }
    }

    resetContent() {
        const classes = [
            '.content-gdpr-accept',
            '.content-gdpr-reject',
            '.content-gdpr-not-chosen'];

        for (const c of classes) {
            document.querySelector(c)?.classList.add('hide');
            document.querySelector(c)?.classList.remove('show');
        }
    }

    showStatus() {
        this.cookieStatus() == null ? 'Niet gekozen' : this.cookieStatus();
    }

    cookieStatus(status) {
        if (status) localStorage.setItem('gdpr-consent-choice', status);
        return localStorage.getItem('gdpr-consent-choice');
    }



    saveMetaData() {
        if (localStorage.getItem('metadata') == null) {
            return;
        }

        else {

            //save time and date of consent in localstorage as a json string
            const date = new Date();

            const meta = {
                date: date.toLocaleDateString(),
                time: date.toLocaleTimeString()
            };
            const metaText = JSON.stringify(meta);
            localStorage.setItem('metadata', metaText);
        }
    }


    hideGDPR() {
        document.querySelector(`.gdpr-consent`).classList.add('hide');
        document.querySelector(`.gdpr-consent`).classList.remove('show');
    }

    showGDPR() {
        document.querySelector(`.gdpr-consent`).classList.add('show');
    }
}

const gdpr = new GDPR();