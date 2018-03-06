using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace BaldaGameWFA
{
    public partial class MainForm : Form
    {
        string letter = "";
        // Valuable to get letter from button on keyboard and set it onto the datagridview.
        string wordToCheck = "";
        // Word, composing after pushing buttons on keyboard by person.
        string pcWord = "";
        // Word, getting while PC turn and setting onto the datagridview.
        string tempSymbol = "";
        // Valuable saving symbol from vertex on datagridview and using only in in case of cancelling the turn.
        const int columnsWidth = 50;
        const int rowsNumber = 5;
        const int lengthOfFirstWord = 5;
        const int middleRow = 2;
        // Number of row for setting startWord's letters before the first turn.
        const int pointColumnNumber = 2;
        const int wordColumnNumber = 1;
        const int wordNumColumnNumber = 0;
        // Numbers of columns for setting №/words/points at datagridviews with points.
        const int startCellNumber = 0;
        const int easyLevel = 1;
        const int middleLevel = 2;
        const int hardLevel = 3;
        int pcLevel;
        // Levels for changing length of words choosing by PC.
        int numLettersAddedToGrid = 0;
        // To prevent cheating by person, typing at not neighbor letters while choosing a word.
        // - 1 value - because of automatical cellmouseclick event even if cellmousedoubleclick is called.
        int personCompleteWords = 0;
        int pcCompleteWords = 0;
        int i;
        int j;
        // i, j - locals to save letter's coordinates on datagridview. 
        int i1;
        int j1;
        // i1, j1 - locals to save letter's coordinates on datagridview after preventing 'cheating' by person. 
        int iCurr;
        int jCurr;
        // iPrev, jPrev - locals to save current letter's coordinates and compare with next selected letter's coordinates. 
        int iPrev;
        int jPrev;
        // iPrev, jPrev - locals to save previous letter's coordinates and check that new-selected-letter is neighbor. 
        bool personAddedLetters = false;
        // Checking adding letter from keyboard onto the grid to prevent 'cheating' by person.
        bool personMadeTurn = true;
        // To allow PC make turn first (if person ins't able to find a word on grid).
        bool wordIncludesAddedLetter = false;
        bool letterWasDeleted = false;
        // To check 'deleting' letter from grid after it's second picking.
        List<string> pcChosenWord = new List<string>();
            // Collection with words chosen by pc for turn. Clears after every turn.
        List<string> dictionary = new List<string>();
            // Creating dictionary for adding all our words before using SortedDictionary
        SortedDictionary<string, int> sortDictionary;
        //int[][] allCoordinates;
        List<Coordinates> allCoordinates= new List<Coordinates>();
            // Saving coordinates of pc's word to return standart color.
        Vertex vert;
        PCTurn pcTurn;    
        
        public struct Coordinates
        {
            public int Row { get; set; }
            public int Col { get; set; }

            public Coordinates(int row, int col)
            {
                Row = row;
                Col = col;
            }
        }

        public MainForm()
        {
            InitializeComponent();
            LevelForm lf = new LevelForm();
            Application.Run(lf);
            pcLevel = lf.PCLevel;
            timer1.Interval = SystemInformation.DoubleClickTime - 1;
            timer1.Tick += new EventHandler(timer1_Tick);
            dataGridViewMain.Rows.Add(rowsNumber);
            for (int i = 0; i < dataGridViewMain.ColumnCount; i++)
            {
                dataGridViewMain.Columns[i].Width = columnsWidth;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            sortDictionary = CreateDictionary();
            LoadFirstWord();
            SetSymbolForFirstTime();
            if (pcLevel == easyLevel)
            {
                easyLevelToolStripMenuItem.BackColor = SystemColors.ControlLight;
            }
            else if (pcLevel == middleLevel)
            {
                middleLevelToolStripMenuItem.BackColor = SystemColors.ControlLight;
            }
            else
            {
                hardLevelToolStripMenuItem.BackColor = SystemColors.ControlLight;
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            dataGridViewMain.ClearSelection();
        }

        private void KeyboardWasClicked(object sender, EventArgs e)
        {
            // Event handler for keyboard button click.
            Button btn = (Button)sender;

            letter = btn.Text;
            buttonPersonTurn.Text = "ДОБАВЬТЕ БУКВУ НА ПОЛЕ";
            dataGridViewMain.ClearSelection();
            buttonPCTurn.ForeColor = Color.Black;
            buttonPCTurn.Text = "ХОД КОМПЬЮТЕРА";
            if (allCoordinates.Count != 0)
            { 
                ChangeCellColorToDefault();
            }            
        }

        private void ButtonPersonTurnWasClicked(object sender, EventArgs e)
            // Event handler for Person turn button ("ХОД ИГРОКА") click.
        {
            if (!FirstTurnIsMade())
            {
                MessageBox.Show("Сначала сделайте ход!");
            }
            else if (!personAddedLetters)
            {
                MessageBox.Show("Вы не можете отменить ход, не добавив буквы на поле");
            }
            else if (!wordIncludesAddedLetter)
            {
                DialogResult result = MessageBox.Show("Вы не можете выбрать слово, не включающее добавленную букву. Набрать слово заново?", "Выберите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                // Changing last letters' coordnates for 'deleting' wrong word coordinates
                {                    
                    iCurr = 0;
                    iPrev = 0;
                    jCurr = 0;
                    jPrev = 0;
                    wordToCheck = "";
                    numLettersAddedToGrid = 0;
                    buttonPersonTurn.Text = "ТЕПЕРЬ ВЫДЕЛИТЕ ПОЛНОЕ СЛОВО";
                    ChangeCellColorToDefault();
                }
                else
                {
                    letterWasDeleted = false;
                    ChangeCellColorToDefault();
                    PersonSkipsTurn(this, e);                    
                }                
            }
            else
            {
                if (sortDictionary.ContainsKey(wordToCheck.ToLower()))
                {
                    wordIncludesAddedLetter = false;
                    letterWasDeleted = false;
                    personMadeTurn = true;
                    personAddedLetters = false;
                    numLettersAddedToGrid = 0;
                    SetNeighborPlusOnDataGrid();                    
                    SetLetterForVertexMass();
                    SetPlusForVertexMass();
                    dataGridViewPerson.Rows.Add();
                    dataGridViewPerson.Rows[personCompleteWords].Cells[wordColumnNumber].Value = wordToCheck.ToLower();
                    dataGridViewPerson.Rows[personCompleteWords].Cells[pointColumnNumber].Value = wordToCheck.Length;
                    dataGridViewPerson.Rows[personCompleteWords].Cells[wordNumColumnNumber].Value = ++personCompleteWords;                    
                    sortDictionary.Remove(wordToCheck.ToLower());
                    if (labelFinalPointsPerson.Text == "")
                    {
                        labelFinalPointsPerson.Text = wordToCheck.Length.ToString();
                    }
                    else
                    {
                        labelFinalPointsPerson.Text = (Int32.Parse(labelFinalPointsPerson.Text) + wordToCheck.Length).ToString();
                    }
                    ChangeCellColorToDefault();
                    dataGridViewPerson.ClearSelection();
                    CheckGridForLastTurn();
                }
                else
                {
                    dataGridViewMain.ClearSelection();
                    buttonPersonTurn.Text = "ХОД ИГРОКА";
                    DialogResult result = MessageBox.Show("Такого слова нет в словаре или оно уже было использовано. Если вы сделали ошибку, - можете переходить. Отмена хода?", "Выберите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        if (personCompleteWords == 0 && pcCompleteWords == 0 && !FirstTurnIsMade())
                        {
                            MessageBox.Show("Невозможно отменить ход - сначала выберите слово");                        
                        }
                        else
                        {
                            // Person'll make turn again.
                            dataGridViewMain.Rows[j].Cells[i].Value = tempSymbol.ToString();
                            letterWasDeleted = false;
                            personAddedLetters = false;
                            wordIncludesAddedLetter = false;
                            personMadeTurn = true;
                            numLettersAddedToGrid = 0;
                            ChangeCellColorToDefault();
                        }
                    }
                    else
                    {                        
                        dataGridViewMain.Rows[j1].Cells[i1].Style.BackColor = System.Drawing.SystemColors.Window;
                        dataGridViewMain.Rows[j1].Cells[i1].Style.ForeColor = Color.Black;
                        PersonSkipsTurn(this, e);
                    }
                }
                iPrev = 0;
                jPrev = 0;
                iCurr = 0;
                jCurr = 0;
                letter = "";
                // Setting to 'default' to check uncorrect turn and show messagebox when uncorrect cell was clicked once.
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            DataGridViewCellEventArgs dgvcea = (DataGridViewCellEventArgs)timer1.Tag;
            // Turn after single click
            if (letter == "")
            {
                MessageBox.Show("Сначала выберите букву для формирования слова");
            }
            else if (!personAddedLetters)
            {
                MessageBox.Show("Вы не поместили букву на поле для формирования слова.\n\nДля этого выберите букву на клавиатуре и сделайте двойной клик по ячейке со знаком '+'");
            }
            else if(LetterSelectedTwice(dgvcea.RowIndex, dgvcea.ColumnIndex))
            {                
                if (!letterWasDeleted)
                {
                    iCurr = iPrev;
                    jCurr = jPrev;
                    MessageBox.Show(String.Format("Вы выбрали '{0}' дважды.\n\nНельзя два раза выбирать букву из одной клетки на протяжении хода.\n\nВы можете продолжить ход, выбрав другую соседнюю букву, или отменить его", dataGridViewMain.Rows[dgvcea.RowIndex].Cells[dgvcea.ColumnIndex].Value.ToString()));
                    string temp = buttonPersonTurn.Text.ToString();
                    temp = temp.Remove(temp.Length - 1);
                    buttonPersonTurn.Text = temp;
                    if (numLettersAddedToGrid == 1)
                        // If we deleted first letter, added within turn.
                    {
                        buttonPersonTurn.Text = "ТЕПЕРЬ ВЫДЕЛИТЕ ПОЛНОЕ СЛОВО";
                        numLettersAddedToGrid = 0;
                    }
                    wordToCheck = wordToCheck.Remove(wordToCheck.Length - 1);
                    dataGridViewMain.Rows[dgvcea.RowIndex].Cells[dgvcea.ColumnIndex].Style.BackColor = System.Drawing.SystemColors.Window;
                    dataGridViewMain.Rows[dgvcea.RowIndex].Cells[dgvcea.ColumnIndex].Style.ForeColor = Color.Black;
                    allCoordinates.Remove(new Coordinates(dgvcea.RowIndex, dgvcea.ColumnIndex));
                    // To enable picking this letter again. For example, in case if user picked letter twice by mistake and wants continue picking word through this cell.
                    letterWasDeleted = true;
                }
            }
            else
            {
                if (numLettersAddedToGrid > 0)
                {
                    iPrev = iCurr;
                    jPrev = jCurr;
                }
                iCurr = dgvcea.ColumnIndex;
                jCurr = dgvcea.RowIndex;
                if (NeighborLetterIsSelected(iCurr, jCurr, iPrev, jPrev))
                {                    
                    dataGridViewMain.Rows[jCurr].Cells[iCurr].Style.BackColor = System.Drawing.SystemColors.Highlight;
                    dataGridViewMain.Rows[jCurr].Cells[iCurr].Style.ForeColor = Color.White;
                    Coordinates localCoordinates = new Coordinates(jCurr, iCurr);
                    allCoordinates.Add(localCoordinates);
                    wordToCheck += dataGridViewMain[iCurr, jCurr].Value;
                    if (buttonPersonTurn.Text == "ТЕПЕРЬ ВЫДЕЛИТЕ ПОЛНОЕ СЛОВО")
                    {
                        buttonPersonTurn.Text = "";
                    }
                    buttonPersonTurn.Text += dataGridViewMain[iCurr, jCurr].Value;
                    numLettersAddedToGrid++;
                }
                else
                {
                    DialogResult result = MessageBox.Show("Вы можете выбрать только соседние буквы. Продолжить ход?", "Выберите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    // Changing last letters' coordnates for 'deleting' wrong word coordinates
                    {
                        iCurr = iPrev;
                        jCurr = jPrev;
                    }
                    else
                    {
                        letterWasDeleted = false;
                        PersonSkipsTurn(this, e);
                        buttonPersonTurn.Text = "ХОД ИГРОКА";
                    }
                }                
            }
            dataGridViewMain.ClearSelection();
        }

        private void dataGridViewMain_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            timer1.Tag = e;
            timer1.Start();
        }

        private void dataGridViewMain_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            timer1.Stop();
            i = e.ColumnIndex;
            j = e.RowIndex;
            //Turn after double click
            if (Char.IsLetter(Char.Parse(dataGridViewMain[i, j].Value.ToString())))
            {
                MessageBox.Show("Нельзя добавлять букву вместо уже существующей");
            }
            else if (letter == "")
            {
                MessageBox.Show("Сначала выберите букву для формирования слова");
            }
            else if (!personAddedLetters)
            {
                // To save previous letter's coordinates for checking accuracy of the turn.                
                i = e.ColumnIndex;
                j = e.RowIndex;

                if (dataGridViewMain[i, j].Value.ToString() == "-")
                {
                    MessageBox.Show("Выбрать букву можно только на клетке со значением '+'");
                    buttonPersonTurn.Text = "ХОД ИГРОКА";
                }
                else
                {
                    wordToCheck = "";
                    // Because of automatical adding '+' at *once method. 
                    tempSymbol = dataGridViewMain[i, j].Value.ToString();
                    personAddedLetters = true;
                    personMadeTurn = false;
                    dataGridViewMain[i, j].Value = letter;
                    buttonPersonTurn.Text = "ТЕПЕРЬ ВЫДЕЛИТЕ ПОЛНОЕ СЛОВО";
                }
            }
            else
            {
                MessageBox.Show("Нельзя поместить более 1 буквы на поле за ход");                
                buttonPersonTurn.Text = "ТЕПЕРЬ ВЫДЕЛИТЕ ПОЛНОЕ СЛОВО";
            }
            dataGridViewMain.ClearSelection();
        }
        
        private SortedDictionary<string, int> CreateDictionary()
            // Creating dictionary from txt file in Resources.
        {
            int dictValuable = 0;
            SortedDictionary<string, int> sortDict = new SortedDictionary<string, int>();
            dictionary = (Properties.Resources.BaldaDictionary.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)).ToList<string>();

            foreach (string word in dictionary)
            {
                sortDict.Add(word, dictValuable);
                if (word.Length == lengthOfFirstWord)
                {
                    pcChosenWord.Add(word);
                }
            }
            return sortDict;
        }

        private void LoadFirstWord()
        {
            Random rand = new Random();
            string wordToCompare = pcChosenWord[rand.Next(0, pcChosenWord.Count - 1)].ToUpper();
            pcTurn = new PCTurn(wordToCompare);
            // Chosing random 5-symbol word from temporary array for making first turn.           
            for (int i = 0; i < dataGridViewMain.ColumnCount; i++)
            // Filling datagridview before the first turn.
            {
                dataGridViewMain.Rows[middleRow].Cells[i].Value = wordToCompare[i];
            }
            if (sortDictionary.ContainsKey(wordToCompare.ToLower()))
            {
                sortDictionary.Remove(wordToCompare.ToLower());
            }
            pcChosenWord.Clear();
        }

        private int ChangeDataGridNumberIntoVertex(int row, int cell)
        {
            return row * rowsNumber + cell;
        }
       
        private void SetSymbolForFirstTime()
        {
            int row = 0;
            int col = 0;

            for (int i = 0; i < dataGridViewMain.RowCount; i++)
            {
                for (int j = 0; j < dataGridViewMain.Columns.Count; j++)
                {
                    dataGridViewMain.Rows[i].Cells[j].Value = "-";
                }
            }            
            vert = pcTurn.GetVert();
            for (int i = 0; i < vert.NumberVertex; i++)
            {
                if (vert.GetVertexLetter(i) == '+' || Char.IsLetter(vert.GetVertexLetter(i)))
                {
                    char symbol = vert.GetVertexLetter(i);
                    row = i / lengthOfFirstWord;
                    col = i % lengthOfFirstWord;
                    dataGridViewMain.Rows[row].Cells[col].Value = symbol.ToString();
                }
            }
        }

        private void SetNeighborPlusOnDataGrid()
            // Setting '+' on neighbor vertexes after person turn.
        {
            if ((i - 1) >= startCellNumber && dataGridViewMain[i - 1, j].Value.ToString() == "-")
            {
                dataGridViewMain[i - 1, j].Value = '+';
            }
            if ((i + 1) < dataGridViewMain.ColumnCount && dataGridViewMain[i + 1, j].Value.ToString() == "-")
            {
                dataGridViewMain[i + 1, j].Value = '+';
            }
            if ((j - 1) >= startCellNumber && dataGridViewMain[i, j - 1].Value.ToString() == "-")
            {
                dataGridViewMain[i, j - 1].Value = '+';
            }
            if ((j + 1) < dataGridViewMain.RowCount && dataGridViewMain[i, j + 1].Value.ToString() == "-")
            {
                dataGridViewMain[i, j + 1].Value = '+';                
            }
            dataGridViewMain.Refresh();            
        }

        private void SetPlusForVertexMass()
            // Changing '-' to '+' at main VertexList in PCTurn class.
        {
            for (int i = 0; i < dataGridViewMain.ColumnCount; i++)
            {
                for (int j = 0; j < dataGridViewMain.RowCount; j++)
                {
                    if (dataGridViewMain[i, j].Value.ToString() == "+" && vert.GetVertexLetter(j * rowsNumber + i) == '-')
                    {
                        vert.SetVertexLetter((j * rowsNumber + i), '+');
                    }
                }
            }
        }

        private void SetLetterForVertexMass()
        {
            if (!Char.IsLetter(vert.GetVertexLetter(j * rowsNumber + i)))
            {
                vert.SetVertexLetter(j * rowsNumber + i, Char.Parse(dataGridViewMain[i, j].Value.ToString()));
            }
        }

        private void RunGamePC(object sender, EventArgs e)
        {
            if (personMadeTurn)
            {
                if (allCoordinates.Count != 0)
                {
                    ChangeCellColorToDefault();
                }
                buttonPersonTurn.Text = "ХОД ИГРОКА";
                List<List<int>> allPathes = pcTurn.PCMakesPathes();
                List<DictionaryKey> dkeysALL = pcTurn.PCSetsPathesToWords(allPathes, sortDictionary);
                if (!pcTurn.KeyListIsEmpty(dkeysALL))
                // If PC has word(s) to make turn
                {
                    object dkeyTemp = pcTurn.PCChooseWordForTurn(pcLevel, dkeysALL, sortDictionary);
                    if (dkeyTemp != null)
                    {
                        DictionaryKey dkey = (DictionaryKey)dkeyTemp;
                        pcWord = dkey.GetWord().ToUpper();
                        List<int> path = dkey.GetListKey();
                        for (int k = 0; k < path.Count; k++)
                        {
                            if (Char.Parse(dataGridViewMain.Rows[path[k] / 5].Cells[path[k] % 5].Value.ToString()) == '+')
                            {
                                j = path[k] / 5;
                                i = path[k] % 5;
                            }
                            dataGridViewMain.Rows[path[k] / 5].Cells[path[k] % 5].Value = pcWord[k];
                            dataGridViewMain.Rows[path[k] / 5].Cells[path[k] % 5].Style.BackColor = Color.Red;
                            Coordinates localCoordinates = new Coordinates(path[k] / 5, path[k] % 5);                         
                            allCoordinates.Add(localCoordinates);
                        }
                        dataGridViewMain.Refresh();
                        buttonPCTurn.ForeColor = Color.Black;
                        buttonPCTurn.Text = "ХОД КОМПЬЮТЕРА";                        
                        SetNeighborPlusOnDataGrid();
                        sortDictionary.Remove(pcWord.ToLower());                        
                        SetLetterForVertexMass();
                        SetPlusForVertexMass();
                        PCGetPoints();
                        CheckGridForLastTurn();
                    }
                    else
                    {
                        PCSkipsTurn();
                    }
                }
                else
                {
                    PCSkipsTurn();
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("Вы не составили слово после того, как поставили букву на поле. Продолжить ход?", "Выберите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                // Person'll continue making a turn.
                {
                    personAddedLetters = true;
                    i1 = i;
                    j1 = j;                    
                    dataGridViewMain.Rows[j1].Cells[i1].Style.BackColor = System.Drawing.SystemColors.Highlight;
                    dataGridViewMain.Rows[j1].Cells[i1].Style.ForeColor = Color.White;
                    dataGridViewMain.ClearSelection();
                    personMadeTurn = false;
                }
                else
                {
                    PersonSkipsTurn(this, e);
                }
            }
        }

        private void PCSkipsTurn()
        {
            buttonPCTurn.ForeColor = Color.Red;
            buttonPCTurn.Text = "СЛОВ НЕ НАЙДЕНО";
        }

        private void PCGetPoints()
            // Setting point got by PC to 'PC-points' datagridview.
        {
            dataGridViewPC.Rows.Add();
            dataGridViewPC.Rows[pcCompleteWords].Cells[wordColumnNumber].Value = pcWord.ToLower();
            dataGridViewPC.Rows[pcCompleteWords].Cells[pointColumnNumber].Value = pcWord.Length;
            dataGridViewPC.Rows[pcCompleteWords].Cells[wordNumColumnNumber].Value = ++pcCompleteWords;
            dataGridViewPC.ClearSelection();
            if (labelFinalPointsPC.Text == "")
            {
                labelFinalPointsPC.Text = pcWord.Length.ToString();
            }
            else
            {
                labelFinalPointsPC.Text = (Int32.Parse(labelFinalPointsPC.Text) + pcWord.Length).ToString();
            }
        }

        private void ChangeCellColorToDefault()
        {
            for (int i = 0; i < allCoordinates.Count; i++)
            {
                dataGridViewMain.Rows[allCoordinates[i].Row].Cells[allCoordinates[i].Col].Style.BackColor = System.Drawing.SystemColors.Window;
                dataGridViewMain.Rows[allCoordinates[i].Row].Cells[allCoordinates[i].Col].Style.ForeColor = Color.Black;
            }
            allCoordinates.Clear();
        } 
        
        private bool FirstTurnIsMade()
        {
            int numOfLetters = 0;
            for (int i = 0; i < dataGridViewMain.RowCount; i++)
            {
                for (int j = 0; j < dataGridViewMain.ColumnCount; j++)
                {
                    if (Char.IsLetter(Char.Parse(dataGridViewMain[j, i].Value.ToString())))
                    {
                        numOfLetters++;
                    }
                }                
            }
            return (numOfLetters == lengthOfFirstWord) ? false : true;
        }

        private bool NeighborLetterIsSelected(int col, int row, int colPrev, int rowPrev)
        // Checking accuracy of selecting neighbor letters on datagridview.
        {
            if (col == i && row == j)
            {
                wordIncludesAddedLetter = true;
            }
            if (numLettersAddedToGrid == 0)
            {
                return true;
            }            
            else if (numLettersAddedToGrid > 0)
            {
                if (((col - 1) >= startCellNumber && (col - 1) == colPrev && row == rowPrev) ||
                    ((col + 1) < dataGridViewMain.ColumnCount && (col + 1) == colPrev && row == rowPrev) ||
                    ((row - 1) >= startCellNumber && col == colPrev && (row - 1 == rowPrev)) ||
                    ((row + 1) < dataGridViewMain.RowCount && col == colPrev && (row + 1 == rowPrev)))
                {
                    return true;
                }
            }
            return false;
        }

        private bool LetterSelectedTwice(int row, int col)
        {
            for (int i = 0; i < allCoordinates.Count; i++)
            {
                if (row == allCoordinates[i].Row && col == allCoordinates[i].Col)
                {
                    return true;
                }
            }
            return false;
        }

        private void PersonSkipsTurn(object sender, EventArgs e)
        {  
            dataGridViewMain.Rows[j].Cells[i].Value = tempSymbol.ToString();
            personAddedLetters = false;
            wordIncludesAddedLetter = false;
            personMadeTurn = true;
            numLettersAddedToGrid = 0;
            dataGridViewMain.ClearSelection();
            RunGamePC(this, e);
        }

        private void exitToolStripMenuItem(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void easyToolStripMenuItem(object sender, EventArgs e)
        {
            pcLevel = easyLevel;
            easyLevelToolStripMenuItem.BackColor = SystemColors.ControlLight;
            middleLevelToolStripMenuItem.BackColor = SystemColors.Control;
            hardLevelToolStripMenuItem.BackColor = SystemColors.Control;
        }

        private void middleToolStripMenuItem(object sender, EventArgs e)
        {
            pcLevel = middleLevel;
            easyLevelToolStripMenuItem.BackColor = SystemColors.Control;
            middleLevelToolStripMenuItem.BackColor = SystemColors.ControlLight;
            hardLevelToolStripMenuItem.BackColor = SystemColors.Control;
        }

        private void hardToolStripMenuItem(object sender, EventArgs e)
        {
            pcLevel = hardLevel;
            easyLevelToolStripMenuItem.BackColor = SystemColors.Control;
            middleLevelToolStripMenuItem.BackColor = SystemColors.Control;
            hardLevelToolStripMenuItem.BackColor = SystemColors.ControlLight;
        }

        private void rulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1. В игре принимают участие человек и компьютер.\n\n2. Первое слово из пяти букв, случайным образом выбранное компьютером, вписывается в центр игрового поля размером 5х5 клеток.\n\n3. Игрок и компьютер могут играть независимо друг от друга.\n\n4. Игрок с помощью одного клика левой клавиши мыши выбирает букву на экранной клавиатуре и с помощью двойного клика указывает клетку игрового поля \"+\", на которую следует поместить выбранную букву.\n\n5. Затем игрок последовательно отмечает буквы нового слова, среди которых обязательно должна быть добавленная буква.\n\n6. Допустимые направления передвижения: вниз, вверх, влево, вправо. Прыжок через клетку и движение по диагонали не допускаются.\n\n7. За каждое новое слово игрок получает столько очков, сколько букв в слове.",
                "Правила игры и порядок пользования интерфейсом",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Игра 'Балда' разработана с демонстрационной целью в рамках изучения платформы .NET", "О приложении", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
            Environment.Exit(0);
        }

        private void endGameButton_Click(object sender, EventArgs e)
        {
            int personResult = 0;
            int pcResult = 0;
            Int32.TryParse(labelFinalPointsPerson.Text.ToString(), out personResult);
            Int32.TryParse(labelFinalPointsPC.Text.ToString(), out pcResult);
            // In case of ending game while labels are empty.
            DialogResult result = MessageBox.Show("Вы действительно хотите завершить эту партию?", "Выберите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            // Changing last letters' coordnates for 'deleting' wrong word coordinates
            {
                MessageBox.Show(String.Format("Итоговый счет: {0} - {1}.\nНажмите 'ОК' для начала новой партии", personResult, pcResult));
                Application.Restart();
                Environment.Exit(0);
            }
        }

        private void CheckGridForLastTurn()
        {
            int countOfLetters = 0;
            for (int i = 0; i < dataGridViewMain.RowCount; i++)
            {
                for (int j = 0; j < dataGridViewMain.Columns.Count; j++)
                {
                    if (Char.IsLetter(Char.Parse(dataGridViewMain.Rows[i].Cells[j].Value.ToString())))
                    {
                        countOfLetters++;
                    }
                }
            }
            if (countOfLetters == 25)
            {
                int personResult = 0;
                int pcResult = 0;
                Int32.TryParse(labelFinalPointsPerson.Text.ToString(), out personResult);
                Int32.TryParse(labelFinalPointsPC.Text.ToString(), out pcResult);
                string finalMessage;
                DialogResult result;

                if (personResult > pcResult)
                {
                    finalMessage = String.Format("Игра закончена, поздравляем с победой!\nИтоговый счет: {0} - {1}. Начать новую партию?", personResult, pcResult);        
                    result = MessageBox.Show(finalMessage, "Выберите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }
                else if(personResult < pcResult)
                {
                    finalMessage = String.Format("Игра закончена, вы проиграли!\nИтоговый счет: {0} - {1}. Начать новую партию?", personResult, pcResult);
                    result = MessageBox.Show(finalMessage, "Выберите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }
                else
                {
                    finalMessage = String.Format("Боевая ничья!\nИтоговый счет: {0} - {1}. Начать новую партию?", personResult, pcResult);
                    result = MessageBox.Show(finalMessage, "Выберите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }                
                if (result == DialogResult.Yes)
                {                    
                    Application.Restart();                    
                }
                Environment.Exit(0);
            }
        }
    }
}