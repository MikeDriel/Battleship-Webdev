import Cell from "./Cell";

class Board extends HTMLElement {

    shadowRoot;
    templateId = 'board-template';
    elementId = 'board';


    constructor() {
        super(); // always call super() first in the ctor.
        this.shadowRoot = this.attachShadow({ mode: 'open' });
        this.container = document.createElement('table');
        this.state = {
            board: [

                [1, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0]


            ]
        };
        this.GenerateBoard();
        this.attachStyling();
    }

    GenerateBoard() {

        for (var i = 0; i < this.state.board.length; i++) {
            let row = document.createElement('tr');
            for (var j = 0; j < this.state.board[i].length; j++) {
                const data = this.state.board[i][j];
                let cell = document.createElement(Cell);
                cell.id = `${i}-${j}`;
                cell.data = data;
                row.appendChild(cell);
            }
            this.container.appendChild(row);
        }
        this.shadowRoot.appendChild(this.container);
    }



    attachStyling() {
        const linkElem = document.createElement("link");
        linkElem.setAttribute("rel", "stylesheet");
        linkElem.setAttribute("href", "/css/BattleShip.css");
        this.shadowRoot.appendChild(linkElem);
    }

}

customElements.define('boat-board', Board);

export default Board;
