using System;
using System.Collections.Generic;
using LIST_TOKENS = ARCalc.Structures.LinkedList<System.String>;
using System.Text;
using ARCalc.Computors;
using ARCalc.Structures;
using ARCalc.Entities;

namespace ARCalc
{
    public class CalcManager
    {
        private CalcState _curState;
        private readonly MainWindow _main;
        private readonly IList<string> _tokens;
        private readonly IRPNParser _parser;
        private readonly Evaluator _eval;
        private int _toks;
        private bool _isPositive;

        private readonly LinkedStack<CharPosition> _parenths;
        public CalcManager(MainWindow mw)
        {
            _curState = CalcState.ZERO;
            _main = mw;
            _tokens = new LIST_TOKENS();
            _toks = 0;
            _parser = new RPNParser();
            _eval = new Evaluator();
            _parenths = new LinkedStack<CharPosition>();
            _isPositive = true;
        }

        public CalcState State { get { return _curState; } }

        /// <summary>
        /// Assume a valid input from user
        /// Select action for input string and execute it.
        /// </summary>
        /// <param name="input">A valid character string. Allowed chars: [0-9]+ or space or [()+-*/C&lt;]</param>
        public void Manage(String input)
        {
            ButtonCode c = GetButtonCode(input);
            switch (c)
            {
                case ButtonCode.ERR:
                    {
                        break;
                    }
                    /* first zero of operand. */
                case ButtonCode.ZERO when (_curState == CalcState.ZERO || _curState == CalcState.OP_PUSHED):
                    {
                        break;
                    }
                    /* first non-zero digit of operand */
                case ButtonCode lc when lc >= ButtonCode.ZERO && lc <= ButtonCode.NINE && (_curState == CalcState.ZERO || _curState == CalcState.OP_PUSHED):
                    {
                        _curState = CalcState.NUM_TYPED;
                        _main.INP.Text = "";
                        _main.INP.Text += input;
                        break;
                    }
                /* try to type first digit after ')' parenthesis. Ignore that because we need an operator. */
                case ButtonCode lc when lc >= ButtonCode.ZERO && (lc <= ButtonCode.NINE || lc == ButtonCode.SPACE || lc == ButtonCode.COMMA) && _curState == CalcState.PAREN_CLOSED:
                    {
                        break;
                    }
                /* second digit or just space symbol of operand */
                case ButtonCode lc when lc >= ButtonCode.ZERO && lc <= ButtonCode.NINE || lc == ButtonCode.SPACE:
                    {
                        _main.INP.Text += input;
                        break;
                    }
                    /* first period symbol */
                case ButtonCode.COMMA when _curState != CalcState.DOT_PUSHED:
                    {
                        _curState = CalcState.DOT_PUSHED;
                        _main.INP.Text += ".";
                        break;
                    }
                    /* open parenthesis '(' before operator */
                case ButtonCode.OP_PAR when _curState == CalcState.OP_PUSHED:
                    {
                        try
                        {
                            _main.INP.Text = "0";
                            _main.EXPR.Text += " ( ";
                            _tokens.Add("(");
                            _toks++;
                            _parenths.Push(new CharPosition('(', _main.EXPR.Text.Length - 1));
                        }
                        /* Set error message and style for EXPR TextBlock */
                        catch (InvalidOperationException)
                        {
                            _main.EXPR.Text = "Error: Exceeded the maximum count of elements (operators and operands).";
                            _curState = CalcState.ERROR;
                            if (_main.Resources["TextBlock_Err"] is System.Windows.Style style)
                            {
                                _main.EXPR.Style = style;
                            }
                        }
                        break;
                    }
                /* open parenthesis '(' right after ')' */
                case ButtonCode.OP_PAR when _curState == CalcState.PAREN_CLOSED:
                    {
                        try
                        {
                            _main.EXPR.Text += " * ( ";
                            _tokens.Add("*");
                            _tokens.Add("(");
                            _curState = CalcState.ZERO;
                            _toks += 2;
                        }
                        catch (InvalidOperationException)
                        {
                            _main.EXPR.Text = "Error: Exceeded the maximum count of elements (operators and operands).";
                            _curState = CalcState.ERROR;
                            if (_main.Resources["TextBlock_Err"] is System.Windows.Style style)
                            {
                                _main.EXPR.Style = style;
                            }
                        }
                        break; 
                    }
                    /* open parenthesis in the begining of input. */
                case ButtonCode.OP_PAR when _curState == CalcState.ZERO:
                    {
                        try
                        {
                            _main.EXPR.Text += "( ";
                            _parenths.Push(new CharPosition('(', _main.EXPR.Text.Length - 1));
                            _tokens.Add("(");
                            _toks += 1;
                        }
                        catch (InvalidOperationException)
                        {
                            _main.EXPR.Text = "Error: Exceeded the maximum count of elements (operators and operands).";
                            _curState = CalcState.ERROR;
                            if (_main.Resources["TextBlock_Err"] is System.Windows.Style style)
                            {
                                _main.EXPR.Style = style;
                            }
                        }
                        break;
                    }
                /* open parenthesis '(' after operand */
                case ButtonCode.OP_PAR:
                    {
                        try
                        {
                            _main.EXPR.Text += _main.INP.Text + " * ( "; /* by default: multiplication is preceeded */
                            _curState = CalcState.ZERO; /* '(' is pseudo operator */
                            _tokens.Add(_main.INP.Text);
                            _tokens.Add("*");
                            _tokens.Add("(");
                            _toks += 3;
                            _main.INP.Text = "0";
                            _parenths.Push(new CharPosition('(', _main.EXPR.Text.Length - 1));
                        }
                        catch (InvalidOperationException)
                        {
                            _main.EXPR.Text = "Error: Exceeded the maximum count of elements (operators and operands).";
                            _curState = CalcState.ERROR;
                            if (_main.Resources["TextBlock_Err"] is System.Windows.Style style)
                            {
                                _main.EXPR.Style = style;
                            }
                        }
                        break;
                    }
                    /* close parenthesis ')' */
                case ButtonCode.CL_PAR:
                    {
                        if (_parenths.IsEmpty()) /* where are no matched '(' symbols */
                        {
                            _main.EXPR.Text = "Error: Attempting to create unbalanced (unmatched) parenthesis.";
                            _curState = CalcState.ERROR;
                            if (_main.Resources["TextBlock_Err"] is System.Windows.Style style)
                            {
                                _main.EXPR.Style = style;
                            }
                        }
                        else
                        {
                            try
                            {
                                _main.EXPR.Text += _main.INP.Text + " ) ";
                                _tokens.Add(_main.INP.Text);
                                _tokens.Add(")");
                                _main.INP.Text = "0";
                                _curState = CalcState.PAREN_CLOSED;
                                _parenths.Pop();
                                _toks += 2;
                            }
                            catch (InvalidOperationException)
                            {
                                _main.EXPR.Text = "Error: Exceeded the maximum count of elements (operators and operands).";
                                _curState = CalcState.ERROR;
                                if (_main.Resources["TextBlock_Err"] is System.Windows.Style style)
                                {
                                    _main.EXPR.Style = style;
                                }
                            }
                            
                        }
                        break;
                    }
                    /* operator [NOT PARENTHESES '(' and ')']*/
                case ButtonCode lc when lc >= ButtonCode.OP_BEGIN && lc <= ButtonCode.OP_END:
                    {
                        if(_curState == CalcState.DOT_PUSHED && _main.INP.Text[^1] == '.')
                        {
                            _main.INP.Text += "0";
                        }
                        else if(_curState == CalcState.PAREN_CLOSED)
                        {
                            try
                            {
                                _tokens.Add(input);
                                _toks += 1;
                                _main.EXPR.Text += " " + input;
                                _main.INP.Text = "0";
                                _curState = CalcState.OP_PUSHED;
                                break;
                            }
                            catch (InvalidOperationException)
                            {
                                _main.EXPR.Text = "Error: Exceeded the maximum count of elements (operators and operands).";
                                _curState = CalcState.ERROR;
                                if (_main.Resources["TextBlock_Err"] is System.Windows.Style style)
                                {
                                    _main.EXPR.Style = style;
                                }
                                break;
                            }
                        }
                        _curState = CalcState.OP_PUSHED;
                        try
                        {
                            _tokens.Add(_main.INP.Text);/* Add operand with operator */
                            _tokens.Add(input);
                            _toks += 2;
                        }
                        catch(InvalidOperationException)
                        {
                            _main.EXPR.Text = "Error: Exceeded the maximum count of elements (operators and operands).";
                            _curState = CalcState.ERROR;
                            if(_main.Resources["TextBlock_Err"] is System.Windows.Style style)
                            {
                                _main.EXPR.Style = style;
                            }
                        }
                        _main.EXPR.Text += " " + _main.INP.Text + " " + input + " ";
                        _main.INP.Text = "0";
                        break;
                    }
                    /* '=' symbol */
                case ButtonCode.EQ:
                    {
                        if (_curState == CalcState.DOT_PUSHED && _main.INP.Text[^1] == '.')
                        {
                            _main.INP.Text += "0";
                        }
                        try
                        {
                            if (!_parenths.IsEmpty()) /* there are unmatched '(' symbols */
                                throw new ArgumentNullException();
                            _tokens.Add(_main.INP.Text);/* Add last operand */
                            _toks += 1;
                            LinkedStack<string> formula = _parser.GetInput(_tokens);
                            _main.INP.Text = _eval.CanParse(formula);
                            _main.EXPR.Text = "";

                            /* Preserve only result at tokens list */
                            _tokens.Clear();
                            _tokens.Add(_main.INP.Text);
                            _isPositive = (_main.INP.Text[0] != '-');
                            _toks = 1;
                        }
                        catch(InvalidOperationException e) when (e.Message == "Eval") {/* TODO: Replace to certain Exception */
                            _main.INP.Text = new string(_main.EXPR.Text + _main.INP.Text);
                            _main.EXPR.Text = "Error: Invalid expresssion.";
                            _curState = CalcState.ERROR;
                            if (_main.Resources["TextBlock_Err"] is System.Windows.Style style)
                            {
                                _main.EXPR.Style = style;
                            }
                        }
                        catch (InvalidOperationException)
                        {
                            _main.EXPR.Text = "Error: Exceeded the maximum count of elements (operators and operands).";
                            _curState = CalcState.ERROR;
                            if (_main.Resources["TextBlock_Err"] is System.Windows.Style style)
                            {
                                _main.EXPR.Style = style;
                            }
                        }
                        catch (ArgumentNullException)/* TODO: MAKE THIS ERROR NOT DRAMMATICAL (DO NOT USE CLEAR COMMAND TO RETURN NORMAL STATE) */
                        {
                            CharPosition unbalanced = _parenths.Top();
                            _main.EXPR.Text = $"Error: Found unmatched open parenthesis \'(\' at symbol: {unbalanced.Pos}";
                            _curState = CalcState.ERROR;
                            if (_main.Resources["TextBlock_Err"] is System.Windows.Style style)
                            {
                                _main.EXPR.Style = style;
                            }
                        }
                        break;
                    }
                case ButtonCode.CLEAR:
                    {
                        _curState = CalcState.ZERO;
                        _main.EXPR.Text = "";
                        _main.INP.Text = "0";
                        /* Erase tokens and parentheses */
                        _tokens.Clear();
                        _toks = 0;
                        _parenths.Clear();
                        _isPositive = true;
                        if (_main.Resources["TextBlock_Normal"] is System.Windows.Style style)
                        {
                            _main.EXPR.Style = style;
                        }
                        break;
                    }
                case ButtonCode.ARROW:
                    {
                        /* str '-0.'  => '0' */
                        if(!_isPositive && _main.INP.Text.Length == 3 && _main.INP.Text[1] == '0' && _main.INP.Text[2] == '.')
                        {
                            _main.INP.Text = "0";
                            _curState = CalcState.ZERO;
                            _isPositive = true;
                        }
                        /* str '5' or '0.' => '0' */
                        if (_isPositive && _main.INP.Text.Length == 1 || (_isPositive && _main.INP.Text.Length == 2 && _main.INP.Text[0] == '0' && _main.INP.Text[1] == '.'))
                        {
                            _main.INP.Text = "0";
                            _curState = CalcState.ZERO;
                        }
                        /* str '-5' => '0' */
                        else if(!_isPositive && _main.INP.Text.Length == 2)/* if is negative and one symbol has remained */
                        {
                            _main.INP.Text = "0";
                            _curState = CalcState.ZERO;
                            _isPositive = true;
                        }
                        else if(_main.INP.Text[^1] == '.')
                        {
                            _curState = CalcState.NUM_TYPED;
                            _main.INP.Text = _main.INP.Text[0..^1];
                        }
                        else
                            _main.INP.Text = _main.INP.Text[0..^1];
                        break;
                    }
                case ButtonCode.SIGN when (_curState == CalcState.NUM_TYPED || _curState == CalcState.DOT_PUSHED) && _isPositive:
                    {
                        _main.INP.Text = "-" + _main.INP.Text;
                        _isPositive = false;
                        break;
                    }
                case ButtonCode.SIGN when _curState == CalcState.NUM_TYPED || _curState == CalcState.DOT_PUSHED:
                    {
                        _main.INP.Text = _main.INP.Text[1..];
                        _isPositive = true;
                        
                        break;
                    }
            }
        }

        private ButtonCode GetButtonCode(String i)
        {
            if (_curState == CalcState.ERROR)
                return i switch
                {
                    "C" => ButtonCode.CLEAR,
                    _ => ButtonCode.ERR
                };
            return i switch
            {
                "0" => ButtonCode.ZERO,
                "1" => ButtonCode.ONE,
                "2" => ButtonCode.TWO,
                "3" => ButtonCode.THREE,
                "4" => ButtonCode.FOUR,
                "5" => ButtonCode.FIVE,
                "6" => ButtonCode.SIX,
                "7" => ButtonCode.SEVEN,
                "8" => ButtonCode.EIGHT,
                "9" => ButtonCode.NINE,
                "+" => ButtonCode.PLUS,
                "-" => ButtonCode.MINUS,
                "*" => ButtonCode.MUL,
                "/" => ButtonCode.DIV,
                "," => ButtonCode.COMMA,
                "." => ButtonCode.COMMA,
                "(" => ButtonCode.OP_PAR,
                ")" => ButtonCode.CL_PAR,
                "C" => ButtonCode.CLEAR,
                "+-" => ButtonCode.SIGN,
                "<-" => ButtonCode.ARROW,
                "=" => ButtonCode.EQ,
                " " => ButtonCode.SPACE,
                "B" => ButtonCode.ARROW,
                _ => ButtonCode.ERR
            };
        }
    }
}
