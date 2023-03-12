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
        //cell.innerText = this.getAttribute('data');
        //cell.style.background = this.GetColor();

        cell.classList.add(this.GetColor());
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