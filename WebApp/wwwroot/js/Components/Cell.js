class Cell extends HTMLElement {

    shadowRoot;
    templateId = 'board-template';
    elementId = 'board';

    constructor() {
        super(); // always call super() first in the ctor.
        this.shadowRoot = this.attachShadow({ mode: 'open' });
        this.container = document.createElement('table');

        //this.CreateCell();
        //this.attachStyling();
    }

    connectedCallback() {
        this.CreateCell();
        this.attachStyling();
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === 'data' && this.shadowRoot.firstElementChild) {
            this.shadowRoot.firstElementChild.className = this.GetColor();
        }
    }

    static get observedAttributes() {
        return ['data'];
    }

    GetColor() {
        switch (parseInt(this.getAttribute('data'))) {
            default:
            case 0:
                return "black"; // black
            case 1:
                return "blue"; // blue
            case 2:
                return "red"; // red
            case 3:
                return "green"; // green
        }
    }

    CreateCell() {
        let cell = document.createElement('td');
        cell.id = this.getAttribute('id');
        cell.classList.add(this.GetColor());

        // Add the click event listener to the 'td' element
        cell.addEventListener('click', (event) => {
            console.log('Cell click event fired');
            const cellClickedEvent = new CustomEvent('cellClicked', {
                detail: { cellId: this.getAttribute('id') },
                bubbles: true,
                composed: true
            });
            this.dispatchEvent(cellClickedEvent);
        });

        this.shadowRoot.appendChild(cell);
    }

    attachStyling() {
        const linkElem = document.createElement("link");
        linkElem.setAttribute("rel", "stylesheet");
        linkElem.setAttribute("href", "/css/BattleShip.css");
        this.shadowRoot.appendChild(linkElem);
    }

}

customElements.define('board-cell', Cell);

export default Cell;