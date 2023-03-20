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
                cell.setAttribute('data', this.state.board[i][j]);
                cell.addEventListener('click', () => {
                    this.handleCellClick(i, j);
                });
                row.appendChild(cell);
            }
            this.container.appendChild(row);
        }
        this.shadowRoot.appendChild(this.container);
    }

    handleCellClick(row, col) {
        // Implement logic to send row and col to the backend
        console.log(`Cell clicked: (${row}, ${col})`);

        // Send the cell coordinates using SignalR
        connection.invoke("Shoot", row, col).catch(err => console.error(err.toString()));
    }


    set boardState(newState) {
        this.state.board = newState;
        this.updateBoard();
    }

    updateBoard() {
        for (let i = 0; i < this.state.board.length; i++) {
            for (let j = 0; j < this.state.board[i].length; j++) {
                const cell = this.shadowRoot.querySelector(`${i}-${j}`);
                if (cell) {
                    cell.setAttribute('data', this.state.board[i][j]);
                }
            }
        }
    }

    updateCell(row, col, cellValue) {
        const cell = this.shadowRoot.getElementById(`${row}-${col}`);
        cell.setAttribute('data', cellValue);
    }


    attachStyling() {
        const linkElem = document.createElement("link");
        linkElem.setAttribute("rel", "stylesheet");
        linkElem.setAttribute("href", "/css/BattleShip.css");
        this.shadowRoot.appendChild(linkElem);
    }

    addEventListeners() {
        for (let i = 0; i < this.state.board.length; i++) {
            for (let j = 0; j < this.state.board[i].length; j++) {
                const cell = this.shadowRoot.getElementById(`${i}-${j}`);
                cell.addEventListener("click", () => {
                    console.log(`Cell clicked: (${i}, ${j})`);
                    // Add your code to send the cell coordinates to the backend
                });
            }
        }
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

    connectedCallback() {
        super.connectedCallback();
        this.addEventListeners();
    }

    addEventListeners() {
        this.addEventListener('cellClicked', (event) => {
            const [row, col] = event.detail.cellId.split('-');
            console.log(`Cell clicked: (${row}, ${col})`);
            // Add your code to send the cell coordinates to the backend
        });
    }
}


//customElements.define('boat-board', BaseBoard);
customElements.define('player1-board', Player1Board);
customElements.define('player2-board', Player2Board);

export default BaseBoard;