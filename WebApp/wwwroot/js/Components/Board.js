class BaseBoard extends HTMLElement {

    shadowRoot;
    templateId = 'board-template';
    elementId = 'board';


    constructor(state = null) {
        super();
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

                let cell = document.createElement('td');
                cell.setAttribute('id', `${i}-${j}`);
                cell.setAttribute('data', this.state.board[i][j]);

                cell.addEventListener('click', () => {
                    var fullCord = cell.getAttribute("id");
                    
                    //split up the cell data
                    var split = fullCord.split("-", 2);
                    var col = split[1];
                    var row = split[0];
                    this.handleCellClick(row, col);
                });

                row.appendChild(cell);
            }
            this.container.appendChild(row);
        }
        this.shadowRoot.appendChild(this.container);
    }

    handleCellClick(row, col) {
        console.log(`Cell clicked: (${row}, ${col})`);

        col = parseInt(col);
        row = parseInt(row);

        //Signal R cord invoke
        connection.invoke("Shoot", row, col).catch(err => console.error(err.toString()));
    }

    updateBoard(newState) {
        this.state.board = newState;

        for (let i = 0; i < this.state.board.length; i++) {
            for (let j = 0; j < this.state.board[i].length; j++) {
                const cell = this.shadowRoot.querySelector(`#${i}-${j}`);
                if (cell) {
                    cell.setAttribute('data', this.state.board[i][j]);

                    // If the cell's state is 2, set the background color to red
                    if (this.state.board[i][j] === 2) {
                        cell.style.backgroundColor = 'red';
                    } else {
                        // Reset the background color for other states if necessary
                        cell.style.backgroundColor = '';
                    }
                }
            }
        }
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

    connectedCallback() {

    }
}


//customElements.define('boat-board', BaseBoard);
customElements.define('player1-board', Player1Board);
customElements.define('player2-board', Player2Board);

export default BaseBoard;
export { Player1Board, Player2Board };
