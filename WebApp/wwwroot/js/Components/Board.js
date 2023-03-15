import Cell from "./Cell.js";

class BaseBoard extends HTMLElement {

    shadowRoot;
    templateId = 'board-template';
    elementId = 'board';


    constructor(state = null) {
        super(); // always call super() first in the ctor.
        this.shadowRoot = this.attachShadow({ mode: 'open' });
        this.container = document.createElement('table');
        this.state = state || {
            board: [

                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],

            ]
        };

        this.GenerateBoard();
        this.attachStyling();
    }

    GenerateBoard() {

        for (var i = 0; i < this.state.board.length; i++) {
            let row = document.createElement('tr');
            for (var j = 0; j < this.state.board[i].length; j++) {

                let cell = document.createElement('board-cell');
                cell.setAttribute('id', `${i}-${j}`);
                cell.setAttribute('data',  this.state.board[i][j]);
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

class Player1Board extends BaseBoard {
    constructor(state = null) {
        super(state);
    }
}

class Player2Board extends BaseBoard {
    constructor(state = null) {
        super(state);
    }
}


//customElements.define('boat-board', BaseBoard);
customElements.define('player1-board', Player1Board);
customElements.define('player2-board', Player2Board);

export default BaseBoard;