class Cell extends HTMLElement {

    shadowRoot;
    templateId = 'board-template';
    elementId = 'board';


    constructor() {
        super(); // always call super() first in the ctor.
        this.shadowRoot = this.attachShadow({ mode: 'open' });
        this.container = document.createElement('table');
        this.state = {
            color: 'black'
        }
        this.GetColor();
        this.CreateCell();
        this.attachStyling();
    }

    GetColor() {
        switch (this.getAttribute('data')) {
            default:
            case 0:
                this.state.color = "black";
                break;
            case 1:
                this.state.color = "blue";
                break;
            case 2:
                this.state.color = "red";
                break;
            case 3:
                this.state.color = "green";
                break;
        }
    }

    CreateCell() {
        let cell = document.createElement('td');
        cell.id = `${this.state.x}-${this.state.y}`;
        cell.innerText = this.state.data;
        cell.style.backgroundColor = this.state.color;
        this.shadowRoot.appendChild(cell);
    }


    attachStyling() {
        const linkElem = document.createElement("link");
        linkElem.setAttribute("rel", "stylesheet");
        linkElem.setAttribute("href", "/css/BattleShip.css");
        this.shadowRoot.appendChild(linkElem);
    }

}

// customElements.define('board-cell', Cell);

export default Cell;
