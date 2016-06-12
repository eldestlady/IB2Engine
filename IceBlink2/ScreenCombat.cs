﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using Color = SharpDX.Color;
using Newtonsoft.Json;

namespace IceBlink2
{
    public class ScreenCombat 
    {
	    public Module mod;
	    public GameView gv;
        public IB2UILayout combatUiLayout = null;
        public bool showHP = false;
        public bool showSP = false;
        public bool showMoveOrder = false;
        public bool showIniBar = true;
        public bool showArrows = true;
        public string nameOfPcTheCreatureMovesTowards = "NoPcTargetChosenYet";
        public Coordinate coordinatesOfPcTheCreatureMovesTowards = new Coordinate(-1, -1);
        public List<Coordinate> storedPathOfCurrentCreature = new List<Coordinate>();
        public int attackAnimationFrameCounter = 0;

        //public int creatureCounter2 = 0;

        //INITIATIVE BAR STUFF
        //each bar holds 34 buttons deactivated at start
        //a bar can contain up to 17 living large or 34 living normal creatures or any mix of these
        //only as many buttons are activated as creatures are contained in current bar, large certaures counted double for this purpose (have 2 buttons)
        //only as many backgroundTiles are drawn as creatures are contained in current bar, normal cretaures counted half for this pupose (have 0.5 background tiles)

        public int numberOfCurrentlyShownBar = 1;

        public List<int> ListOfCreaturesDisplayedInBar1 = new List<int>();
        public List<int> ListOfCreaturesDisplayedInBar2 = new List<int>();
        public List<int> ListOfCreaturesDisplayedInBar3 = new List<int>();
        public List<int> ListOfCreaturesDisplayedInBar4 = new List<int>();
        public List<int> ListOfCreaturesDisplayedInBar5 = new List<int>();
        public List<int> ListOfCreaturesDisplayedInBar6 = new List<int>();

        public List<int >ListOfSizesOfCreaturesInBar1 = new List<int>();
        public List<int> ListOfSizesOfCreaturesInBar2 = new List<int>();
        public List<int> ListOfSizesOfCreaturesInBar3 = new List<int>();
        public List<int> ListOfSizesOfCreaturesInBar4 = new List<int>();
        public List<int> ListOfSizesOfCreaturesInBar5 = new List<int>();
        public List<int> ListOfSizesOfCreaturesInBar6 = new List<int>();

        public List<int> ListOfCreaturesDisplayedInCurrentBar = new List<int>();
        public List<int> ListOfSizesOfCreaturesInCurrentBar = new List<int>();

        public int NumberOfButtonsDisplayedInBar1 = 0;
        public int NumberOfButtonsDisplayedInBar2 = 0;
        public int NumberOfButtonsDisplayedInBar3 = 0;
        public int NumberOfButtonsDisplayedInBar4 = 0;
        public int NumberOfButtonsDisplayedInBar5 = 0;
        public int NumberOfButtonsDisplayedInBar6 = 0;

        public int NumberOfBackgroundTilesDisplayedInBar1 = 0;
        public int NumberOfBackgroundTilesDisplayedInBar2 = 0;
        public int NumberOfBackgroundTilesDisplayedInBar3 = 0;
        public int NumberOfBackgroundTilesDisplayedInBar4 = 0;
        public int NumberOfBackgroundTilesDisplayedInBar5 = 0;
        public int NumberOfBackgroundTilesDisplayedInBar6 = 0;

        public List<int> moverOrdersOfAllLivingCreatures = new List<int>();
        public List<int> moverOrdersOfAllFallenCreatures = new List<int>();
        public List<int> moverOrdersOfLargeLivingCreatures = new List<int>();
        public List<int> moverOrdersOfLargeFallenCreatures = new List<int>();
        public List<int> moverOrdersOfNormalLivingCreatures = new List<int>();
        public List<int> moverOrdersOfNormalFallenCreatures = new List<int>();

        //COMBAT STUFF
        public bool adjustCamToRangedCreature = false;
	    private bool isPlayerTurn = true;
	    public bool canMove = true;
	    public int currentPlayerIndex = 0;
        public int creatureIndex = 0;
        public int currentMoveOrderIndex = 0;
        public List<MoveOrder> moveOrderList = new List<MoveOrder>();
        public int initialMoveOrderListSize = 0;
        public float currentMoves = 0;
        public float creatureMoves = 0;
        public Coordinate UpperLeftSquare = new Coordinate();
	    public string currentCombatMode = "info"; //info, move, cast, attack
	    public Coordinate targetHighlightCenterLocation = new Coordinate();
	    public Coordinate creatureTargetLocation = new Coordinate();
	    private int encounterXP = 0;
	    private Creature creatureToAnimate = null;
	    private Player playerToAnimate = null;
	    //private bool drawHitAnimation = false;
	    //private bool drawMissAnimation = false;
	    private Coordinate hitAnimationLocation = new Coordinate();
	    public int spellSelectorIndex = 0;
	    public List<string> spellSelectorSpellTagList = new List<string>();
	    //private bool drawProjectileAnimation = false;
	    private Coordinate projectileAnimationLocation = new Coordinate();
	    //private bool drawEndingAnimation = false;
	    private Coordinate endingAnimationLocation = new Coordinate();
        public bool drawDeathAnimation = true;
        public List<Coordinate> deathAnimationLocations = new List<Coordinate>();
	    private int animationFrameIndex = 0;
	    public PathFinderEncounters pf;
	    public bool floatyTextOn = false;
	    public AnimationState animationState = AnimationState.None;
	    //private Bitmap projectile;
        //private bool projectileFacingUp = true;
	    //private Bitmap ending_fx;
        private Bitmap mapBitmap;

        /*private IbbButton btnSelect = null;
	    private IbbButton btnMove = null;
	    private IbbButton btnAttack = null;
	    private IbbButton btnCast = null;
	    private IbbButton btnSkipTurn = null;
	    private IbbButton btnSwitchWeapon = null;
        private IbbButton btnMoveCounter = null;
	    public IbbToggleButton tglHP = null;
	    public IbbToggleButton tglSP = null;
        public IbbToggleButton tglMoveOrder = null;
	    public IbbToggleButton tglSpeed = null;
	    public IbbToggleButton tglSoundFx = null;
	    public IbbToggleButton tglKill = null;
	    public IbbToggleButton tglHelp = null;
	    public IbbToggleButton tglGrid = null;*/
        public int mapStartLocXinPixels;
        public float moveCost = 1.0f;
        public List<Sprite> spriteList = new List<Sprite>();
        public List<AnimationSequence> animationSeqStack = new List<AnimationSequence>();
        public bool animationsOn = false;
        public int attackAnimationTimeElapsed = 0;
        public int attackAnimationLengthInMilliseconds = 250;

        public ScreenCombat(Module m, GameView g)
	    {
		    mod = m;
		    gv = g;
            mapStartLocXinPixels = 0 * gv.squareSize;
            loadMainUILayout();
            //CalculateUpperLeft();
            //setControlsStart();
            //setToggleButtonsStart();
	    }

        public void loadMainUILayout()
        {
            try
            {
                if (File.Exists(gv.cc.GetModulePath() + "\\data\\CombatUILayout.json"))
                {
                    using (StreamReader file = File.OpenText(gv.cc.GetModulePath() + "\\data\\CombatUILayout.json"))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        combatUiLayout = (IB2UILayout)serializer.Deserialize(file, typeof(IB2UILayout));
                        combatUiLayout.setupIB2UILayout(gv);
                    }
                }
                else
                {
                    using (StreamReader file = File.OpenText(gv.mainDirectory + "\\default\\NewModule\\data\\CombatUILayout.json"))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        combatUiLayout = (IB2UILayout)serializer.Deserialize(file, typeof(IB2UILayout));
                        combatUiLayout.setupIB2UILayout(gv);
                    }
                }

                IB2ToggleButton tgl = combatUiLayout.GetToggleByTag("tglHP");
                if (tgl != null)
                {
                    showHP = tgl.toggleOn;
                }
                IB2ToggleButton tgl2 = combatUiLayout.GetToggleByTag("tglSP");
                if (tgl2 != null)
                {
                    showSP = tgl2.toggleOn;
                }
                foreach (IB2Panel pnlC in combatUiLayout.panelList)
                {
                    if (pnlC.tag.Equals("logPanel"))
                    {
                        foreach (IB2Panel pnlM in gv.screenMainMap.mainUiLayout.panelList)
                        {
                            if (pnlM.tag.Equals("logPanel"))
                            {
                                pnlC.logList[0] = pnlM.logList[0];
                            }
                        }
                    }

                    if (gv.mod.useMinimalisticUI)
                    {
                        if (pnlC.tag.Equals("arrowPanel"))
                        {
                            pnlC.hiding = true;
                            pnlC.showing = false;
                            showArrows = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Loading CombatUILayout.json: " + ex.ToString());
                gv.errorLog(ex.ToString());
            }
        }

        /*public void setControlsStart()
	    {		
		    int pW = (int)((float)gv.screenWidth / 100.0f);
		    int pH = (int)((float)gv.screenHeight / 100.0f);
		    int padW = gv.squareSize/6;
            int hotkeyShift = 0;
            if (gv.useLargeLayout)
            {
                hotkeyShift = 1;
            }


            if (btnSelect == null)
		    {
			    btnSelect = new IbbButton(gv, 0.8f);	
			    btnSelect.Text = "SELECT";
			    btnSelect.Img = gv.cc.LoadBitmap("btn_small"); // BitmapFactory.decodeResource(getResources(), R.drawable.btn_small);
			    btnSelect.Glow = gv.cc.LoadBitmap("btn_small_glow"); // BitmapFactory.decodeResource(getResources(), R.drawable.btn_small_glow);
                btnSelect.X = gv.cc.pnlArrows.LocX + 1 * gv.squareSize + gv.squareSize / 2;
                btnSelect.Y = gv.cc.pnlArrows.LocY + 1 * gv.squareSize + gv.pS;
                btnSelect.Height = (int)(gv.ibbheight * gv.screenDensity);
                btnSelect.Width = (int)(gv.ibbwidthR * gv.screenDensity);
		    }			
		    if (btnMove == null)
		    {
			    btnMove = new IbbButton(gv, 0.8f);
			    btnMove.Img = gv.cc.LoadBitmap("btn_small");
                btnMove.ImgOn = gv.cc.LoadBitmap("btn_small_on");
                btnMove.ImgOff = gv.cc.LoadBitmap("btn_small_off");
			    btnMove.Glow = gv.cc.LoadBitmap("btn_small_glow");
			    btnMove.Text = "MOVE";
                btnMove.HotKey = "M";
                btnMove.X = gv.cc.pnlHotkeys.LocX + (hotkeyShift + 3) * gv.squareSize;
                btnMove.Y = gv.cc.pnlHotkeys.LocY + 0 * gv.squareSize + gv.pS;
                btnMove.Height = (int)(gv.ibbheight * gv.screenDensity);
                btnMove.Width = (int)(gv.ibbwidthR * gv.screenDensity);			
		    }
            if (btnMoveCounter == null)
            {
                btnMoveCounter = new IbbButton(gv, 1.2f);
                btnMoveCounter.Img = gv.cc.LoadBitmap("btn_small_off"); // BitmapFactory.decodeResource(getResources(), R.drawable.btn_small);
                btnMoveCounter.Glow = gv.cc.LoadBitmap("btn_small_glow"); // BitmapFactory.decodeResource(getResources(), R.drawable.btn_small_glow);
                btnMoveCounter.Text = "0";
                btnMoveCounter.X = gv.cc.pnlHotkeys.LocX + (hotkeyShift + 6) * gv.squareSize;
                btnMoveCounter.Y = gv.cc.pnlHotkeys.LocY + 0 * gv.squareSize + gv.pS;
                btnMoveCounter.Height = (int)(gv.ibbheight * gv.screenDensity);
                btnMoveCounter.Width = (int)(gv.ibbwidthR * gv.screenDensity);
            }
		    if (btnAttack == null)
		    {
			    btnAttack = new IbbButton(gv, 0.8f);
			    btnAttack.Img = gv.cc.LoadBitmap("btn_small");
                btnAttack.ImgOn = gv.cc.LoadBitmap("btn_small_on");
                btnAttack.ImgOff = gv.cc.LoadBitmap("btn_small_off");
			    btnAttack.Glow = gv.cc.LoadBitmap("btn_small_glow"); // BitmapFactory.decodeResource(getResources(), R.drawable.btn_small_glow);
			    btnAttack.Text = "ATTACK";
                btnAttack.HotKey = "A";
                btnAttack.X = gv.cc.pnlHotkeys.LocX + (hotkeyShift + 4) * gv.squareSize;
                btnAttack.Y = gv.cc.pnlHotkeys.LocY + 0 * gv.squareSize + gv.pS;
                btnAttack.Height = (int)(gv.ibbheight * gv.screenDensity);
                btnAttack.Width = (int)(gv.ibbwidthR * gv.screenDensity);			
		    }
		    if (btnCast == null)
		    {
			    btnCast = new IbbButton(gv, 0.8f);
			    btnCast.Img = gv.cc.LoadBitmap("btn_small");
                btnCast.ImgOn = gv.cc.LoadBitmap("btn_small_on");
                btnCast.ImgOff = gv.cc.LoadBitmap("btn_small_off");
			    btnCast.Glow = gv.cc.LoadBitmap("btn_small_glow"); // BitmapFactory.decodeResource(getResources(), R.drawable.btn_small_glow);
			    btnCast.Text = "CAST";
                btnCast.HotKey = "C";
                btnCast.X = gv.cc.pnlHotkeys.LocX + (hotkeyShift + 5) * gv.squareSize;
                btnCast.Y = gv.cc.pnlHotkeys.LocY + 0 * gv.squareSize + gv.pS;
                btnCast.Height = (int)(gv.ibbheight * gv.screenDensity);
                btnCast.Width = (int)(gv.ibbwidthR * gv.screenDensity);			
		    }
		    if (btnSkipTurn == null)
		    {
			    btnSkipTurn = new IbbButton(gv, 0.8f);
			    btnSkipTurn.Img = gv.cc.LoadBitmap("btn_small"); // BitmapFactory.decodeResource(getResources(), R.drawable.btn_small);
			    btnSkipTurn.Glow = gv.cc.LoadBitmap("btn_small_glow"); // BitmapFactory.decodeResource(getResources(), R.drawable.btn_small_glow);
			    btnSkipTurn.Text = "SKIP";
                btnSkipTurn.HotKey = "S";
                btnSkipTurn.X = gv.cc.pnlHotkeys.LocX + (hotkeyShift + 2) * gv.squareSize;
                btnSkipTurn.Y = gv.cc.pnlHotkeys.LocY + 0 * gv.squareSize + gv.pS;
                btnSkipTurn.Height = (int)(gv.ibbheight * gv.screenDensity);
                btnSkipTurn.Width = (int)(gv.ibbwidthR * gv.screenDensity);			
		    }
		    if (btnSwitchWeapon == null)
		    {
			    btnSwitchWeapon = new IbbButton(gv, 0.8f);
                btnSwitchWeapon.HotKey = "P";
			    btnSwitchWeapon.Img = gv.cc.LoadBitmap("btn_small"); // BitmapFactory.decodeResource(getResources(), R.drawable.btn_small);
			    btnSwitchWeapon.Img2 = gv.cc.LoadBitmap("btnparty"); // BitmapFactory.decodeResource(getResources(), R.drawable.btnparty);
			    btnSwitchWeapon.Glow = gv.cc.LoadBitmap("btn_small_glow"); // BitmapFactory.decodeResource(getResources(), R.drawable.btn_small_glow);
                btnSwitchWeapon.X = gv.cc.pnlHotkeys.LocX + (hotkeyShift + 0) * gv.squareSize;
                btnSwitchWeapon.Y = gv.cc.pnlHotkeys.LocY + 0 * gv.squareSize + gv.pS;
                btnSwitchWeapon.Height = (int)(gv.ibbheight * gv.screenDensity);
                btnSwitchWeapon.Width = (int)(gv.ibbwidthR * gv.screenDensity);			
					
		    }
	    }*/
        /*public void setToggleButtonsStart()
        {
    	    int pW = (int)((float)gv.screenWidth / 100.0f);
		    int pH = (int)((float)gv.screenHeight / 100.0f);
		    int padW = gv.squareSize/6;
			
		    if (tglGrid == null)
		    {
			    tglGrid = new IbbToggleButton(gv);
			    tglGrid.ImgOn = gv.cc.LoadBitmap("tgl_grid_on");
			    tglGrid.ImgOff = gv.cc.LoadBitmap("tgl_grid_off");
                tglGrid.X = gv.cc.pnlToggles.LocX + 1 * gv.squareSize + gv.squareSize / 4;
                tglGrid.Y = gv.cc.pnlToggles.LocY + 1 * gv.squareSize + gv.squareSize / 4 + gv.pS;
                tglGrid.Height = (int)(gv.ibbheight / 2 * gv.screenDensity);
                tglGrid.Width = (int)(gv.ibbwidthR / 2 * gv.screenDensity);
			    tglGrid.toggleOn = true;
		    }
		    if (tglHP == null)
		    {
			    tglHP = new IbbToggleButton(gv);
			    tglHP.ImgOn = gv.cc.LoadBitmap("tgl_hp_on"); // BitmapFactory.decodeResource(getResources(), R.drawable.tgl_hp_on);
			    tglHP.ImgOff = gv.cc.LoadBitmap("tgl_hp_off"); // BitmapFactory.decodeResource(getResources(), R.drawable.tgl_hp_off);
                tglHP.X = gv.cc.pnlToggles.LocX + 1 * gv.squareSize + gv.squareSize / 4;
                tglHP.Y = gv.cc.pnlToggles.LocY + 0 * gv.squareSize + gv.squareSize / 4 + gv.pS;
                tglHP.Height = (int)(gv.ibbheight / 2 * gv.screenDensity);
                tglHP.Width = (int)(gv.ibbwidthR / 2 * gv.screenDensity);
		    }
		    if (tglSP == null)
		    {
			    tglSP = new IbbToggleButton(gv);
			    tglSP.ImgOn = gv.cc.LoadBitmap("tgl_sp_on"); // BitmapFactory.decodeResource(getResources(), R.drawable.tgl_sp_on);
			    tglSP.ImgOff = gv.cc.LoadBitmap("tgl_sp_off"); // BitmapFactory.decodeResource(getResources(), R.drawable.tgl_sp_off);
                tglSP.X = gv.cc.pnlToggles.LocX + 2 * gv.squareSize + gv.squareSize / 4;
                tglSP.Y = gv.cc.pnlToggles.LocY + 0 * gv.squareSize + gv.squareSize / 4 + gv.pS;
                tglSP.Height = (int)(gv.ibbheight / 2 * gv.screenDensity);
                tglSP.Width = (int)(gv.ibbwidthR / 2 * gv.screenDensity);
		    }
            if (tglMoveOrder == null)
            {
                tglMoveOrder = new IbbToggleButton(gv);
                tglMoveOrder.ImgOn = gv.cc.LoadBitmap("tgl_mo_on"); // BitmapFactory.decodeResource(getResources(), R.drawable.tgl_sp_on);
                tglMoveOrder.ImgOff = gv.cc.LoadBitmap("tgl_mo_off"); // BitmapFactory.decodeResource(getResources(), R.drawable.tgl_sp_off);
                tglMoveOrder.X = gv.cc.pnlToggles.LocX + 3 * gv.squareSize + gv.squareSize / 4;
                tglMoveOrder.Y = gv.cc.pnlToggles.LocY + 0 * gv.squareSize + gv.squareSize / 4 + gv.pS;
                tglMoveOrder.Height = (int)(gv.ibbheight / 2 * gv.screenDensity);
                tglMoveOrder.Width = (int)(gv.ibbwidthR / 2 * gv.screenDensity);
            }
		    if (tglSpeed == null)
		    {
			    tglSpeed = new IbbToggleButton(gv);
			    tglSpeed.ImgOn = gv.cc.LoadBitmap("tgl_speed_4"); // BitmapFactory.decodeResource(getResources(), R.drawable.tgl_sp_on);
			    tglSpeed.ImgOff = gv.cc.LoadBitmap("tgl_speed_4"); // BitmapFactory.decodeResource(getResources(), R.drawable.tgl_sp_off);
                tglSpeed.X = gv.cc.pnlToggles.LocX + 3 * gv.squareSize + gv.squareSize / 4;
                tglSpeed.Y = gv.cc.pnlToggles.LocY + 2 * gv.squareSize + gv.squareSize / 4 + gv.pS;
                tglSpeed.Height = (int)(gv.ibbheight / 2 * gv.screenDensity);
                tglSpeed.Width = (int)(gv.ibbwidthR / 2 * gv.screenDensity);
		    }
		    if (tglSoundFx == null)
		    {
			    tglSoundFx = new IbbToggleButton(gv);
			    tglSoundFx.ImgOn = gv.cc.LoadBitmap("tgl_sound_on"); // BitmapFactory.decodeResource(getResources(), R.drawable.tgl_sp_on);
			    tglSoundFx.ImgOff = gv.cc.LoadBitmap("tgl_sound_off"); // BitmapFactory.decodeResource(getResources(), R.drawable.tgl_sp_off);
                tglSoundFx.X = gv.cc.pnlToggles.LocX + 3 * gv.squareSize + gv.squareSize / 4;
                tglSoundFx.Y = gv.cc.pnlToggles.LocY + 1 * gv.squareSize + gv.squareSize / 4 + gv.pS;
                tglSoundFx.Height = (int)(gv.ibbheight / 2 * gv.screenDensity);
                tglSoundFx.Width = (int)(gv.ibbwidthR / 2 * gv.screenDensity);
		    }
		    if (tglHelp == null)
		    {
			    tglHelp = new IbbToggleButton(gv);
			    tglHelp.ImgOn = gv.cc.LoadBitmap("tgl_help_on"); // BitmapFactory.decodeResource(getResources(), R.drawable.tgl_sp_on);
			    tglHelp.ImgOff = gv.cc.LoadBitmap("tgl_help_on"); // BitmapFactory.decodeResource(getResources(), R.drawable.tgl_sp_off);
                tglHelp.X = gv.cc.pnlToggles.LocX + 2 * gv.squareSize + gv.squareSize / 4;
                tglHelp.Y = gv.cc.pnlToggles.LocY + 2 * gv.squareSize + gv.squareSize / 4 + gv.pS;
                tglHelp.Height = (int)(gv.ibbheight / 2 * gv.screenDensity);
                tglHelp.Width = (int)(gv.ibbwidthR / 2 * gv.screenDensity);
		    }
		    if (tglKill == null)
		    {
			    tglKill = new IbbToggleButton(gv);
			    tglKill.ImgOn = gv.cc.LoadBitmap("tgl_kill_on"); // BitmapFactory.decodeResource(getResources(), R.drawable.tgl_sp_on);
			    tglKill.ImgOff = gv.cc.LoadBitmap("tgl_kill_on"); // BitmapFactory.decodeResource(getResources(), R.drawable.tgl_sp_off);
                tglKill.X = gv.cc.pnlToggles.LocX + 1 * gv.squareSize + gv.squareSize / 4;
                tglKill.Y = gv.cc.pnlToggles.LocY + 2 * gv.squareSize + gv.squareSize / 4 + gv.pS;
                tglKill.Height = (int)(gv.ibbheight / 2 * gv.screenDensity);
                tglKill.Width = (int)(gv.ibbwidthR / 2 * gv.screenDensity);
		    }
        }*/
        /*public void resetToggleButtons()
        {
            if (mod.combatAnimationSpeed == 100)
            {
                gv.cc.DisposeOfBitmap(ref tglSpeed.ImgOff);
                tglSpeed.ImgOff = gv.cc.LoadBitmap("tgl_speed_1");
            }
            else if (mod.combatAnimationSpeed == 50)
            {
                gv.cc.DisposeOfBitmap(ref tglSpeed.ImgOff);
                tglSpeed.ImgOff = gv.cc.LoadBitmap("tgl_speed_2");
            }
            else if (mod.combatAnimationSpeed == 25)
            {
                gv.cc.DisposeOfBitmap(ref tglSpeed.ImgOff);
                tglSpeed.ImgOff = gv.cc.LoadBitmap("tgl_speed_4");
            }
            else if (mod.combatAnimationSpeed == 10)
            {
                gv.cc.DisposeOfBitmap(ref tglSpeed.ImgOff);
                tglSpeed.ImgOff = gv.cc.LoadBitmap("tgl_speed_10");
            }

            if (mod.playMusic)
            {
                gv.cc.tglSound.toggleOn = true;
            }
            else
            {
                gv.cc.tglSound.toggleOn = false;
            }

            if (mod.playSoundFx)
            {
                tglSoundFx.toggleOn = true;
            }
            else
            {
                tglSoundFx.toggleOn = false;
            }
        }*/
	    public void tutorialMessageCombat(bool helpCall)
        {
    	    if ((mod.showTutorialCombat) || (helpCall))
		    {
			    gv.sf.MessageBoxHtml(
					    "<big><b>COMBAT</b></big><br><br>" +
			
					    "<b>1. Player's Turn:</b> Each player takes a turn. The current player will be highlighted with a" +
					    " light blue box. You can Move one square (or stay put) and make one additional action such" + 
					    " as ATTACK, CAST, use item, or end turn (SKIP button).<br><br>" +
					
					    "<b>2. Info Mode:</b> Info mode is the default mode. In this mode you can tap on a token (player or enemy image) to show" + 
					    " some of their stats (HP, SP, etc.). If none of the buttons are highlighted, then you are in 'Info' mode. If you are" +
					    " in 'move' mode and want to return to 'info' mode, tap on the move button to unselect it and return to 'info' mode. Same" +
					    " concept works for 'attack' mode back to 'info' mode.<br><br>" +
					
					    "<b>3. Move:</b> After pressing move, you may move one square and then do one more action or press 'SKIP' to end this Player's" +
					    " turn. You move by pressing one of the arrow direction buttons or tapping on a square adjacent to the PC.<br><br>" +
					
					    "<b>3. Attack:</b> After pressing attack, move the target selection square by pressing the arrow keys or tapping on any map square." + 
					    " Once you have selected a valid target (box will be green), press the 'TARGET' button or tap on the targeted map square (green box)" +
					    " again to complete the action.<br><br>" +
					
					    "<b>4. Cast:</b> After pressing cast and selecting a spell from the spell selection screen, move the target selection square by" +
					    " pressing the arrow keys or tapping on any map square. Once you have selected a valid target (box will be green), press the" + 
					    " 'TARGET' button or tap on the targeted map square (green box) again to complete the action.<br><br>" +
					
					    "<b>5. Skip:</b> The 'SKIP' button will end the current player's turn.<br><br>" +
					
					    "<b>6. Use Item:</b> press the inventory button (image of a bag) to show the party inventory screen. Only the current Player" +
					    " may use an item from this screen during combat.<br><br>" +
					
					    "<small><b>Note:</b> Also, check out the 'Player's Guide' in the settings menu (the rusty gear looking button)</small>"
					    );
			    mod.showTutorialCombat = false;
		    }
        }
	    
        public void doAnimationController()
	    {
            if (animationState == AnimationState.None)
		    {
			    return;
            }            
            else if (animationState == AnimationState.CreatureThink)
            {
                creatureToAnimate = null;
                playerToAnimate = null;
                doCreatureTurnAfterDelay();
            }
            else if (animationState == AnimationState.CreatureMove)
            {
                creatureToAnimate = null;
                playerToAnimate = null;
                Creature crt = mod.currentEncounter.encounterCreatureList[creatureIndex];
                if (moveCost == mod.diagonalMoveCost)
                {
                    creatureMoves += mod.diagonalMoveCost;
                    moveCost = 1.0f;
                }
                else
                {
                    creatureMoves++;
                }
                doCreatureNextAction();
            }
        }
        
        public void doCombatSetup()
        {
            
            if (mod.playMusic)
            {
                gv.stopMusic();
                gv.stopAmbient();
                gv.startCombatMusic();
            }
            gv.screenType = "combat";
            //resetToggleButtons();
            //Load map if used
            if (mod.currentEncounter.UseMapImage)
            {
                gv.cc.DisposeOfBitmap(ref mapBitmap);
                mapBitmap = gv.cc.LoadBitmap(mod.currentEncounter.MapImage);
            }
            else //loads only the tiles that are used on this encounter map
            {
                //TODO gv.cc.LoadTileBitmapList();
            }
            //Load up all creature stuff
            foreach (CreatureRefs crf in mod.currentEncounter.encounterCreatureRefsList)
            {
                //find this creatureRef in mod creature list
                foreach (Creature c in gv.mod.moduleCreaturesList)
                {
                    if (crf.creatureResRef.Equals(c.cr_resref))
                    {
                        //copy it and add to encounters creature object list
                        try
                        {
                            Creature copy = c.DeepCopy();
                            copy.cr_tag = crf.creatureTag;
                            gv.cc.DisposeOfBitmap(ref copy.token);
                            copy.token = gv.cc.LoadBitmap(copy.cr_tokenFilename);
                            copy.combatLocX = crf.creatureStartLocationX;
                            copy.combatLocY = crf.creatureStartLocationY;
                            mod.currentEncounter.encounterCreatureList.Add(copy);
                        }
                        catch (Exception ex)
                        {
                            gv.errorLog(ex.ToString());
                        }
                    }
                }
            }

            foreach (Creature crt in mod.currentEncounter.encounterCreatureList)
            {
                int decider = gv.sf.RandInt(2);
                if (decider == 1)
                {
                    crt.goDown = false;
                }
                else
                {
                    crt.goDown = true;
                }

                decider = gv.sf.RandInt(2);
                if (decider == 1)
                {
                    crt.goRight = false;
                }
                else
                {
                    crt.goRight = true;
                }
            }

            //Place all PCs
            for (int index = 0; index < mod.playerList.Count; index++)
            {
                mod.playerList[index].combatLocX = mod.currentEncounter.encounterPcStartLocations[index].X;
                mod.playerList[index].combatLocY = mod.currentEncounter.encounterPcStartLocations[index].Y;
            }
            isPlayerTurn = true;
            currentPlayerIndex = 0;
            creatureIndex = 0;
            currentMoveOrderIndex = 0;
            currentCombatMode = "info";
            drawDeathAnimation = false;
            encounterXP = 0;
            if (gv.mod.useManualCombatCam)
            {
                CenterScreenOnPC();
            }
            foreach (Creature crtr in mod.currentEncounter.encounterCreatureList)
            {
                encounterXP += crtr.cr_XP;
            }
            pf = new PathFinderEncounters(mod);
            tutorialMessageCombat(false);
            //IBScript Setup Combat Hook (run only once)
            gv.cc.doIBScriptBasedOnFilename(gv.mod.currentEncounter.OnSetupCombatIBScript, gv.mod.currentEncounter.OnSetupCombatIBScriptParms);
            //IBScript Start Combat Round Hook
            gv.cc.doIBScriptBasedOnFilename(gv.mod.currentEncounter.OnStartCombatRoundIBScript, gv.mod.currentEncounter.OnStartCombatRoundIBScriptParms);
            //determine initiative
            calcualteMoveOrder();
            //do turn controller
            recalculateCreaturesShownInInitiativeBar();
            turnController();
        }
        public void calcualteMoveOrder()
        {
            moveOrderList.Clear();
            //creatureCounter2 = 0;
            //go through each PC and creature and make initiative roll
            foreach (Player pc in mod.playerList)
            {
               
                    int roll = gv.sf.RandInt(100) + (((pc.dexterity - 10) / 2) * 5);
                    MoveOrder newMO = new MoveOrder();
                    newMO.PcOrCreature = pc;
                    newMO.rank = roll;
                    moveOrderList.Add(newMO);
                

            }
            foreach (Creature crt in mod.currentEncounter.encounterCreatureList)
            {
                
                
                    int roll = gv.sf.RandInt(100) + (crt.initiativeBonus * 5);
                    MoveOrder newMO = new MoveOrder();
                    newMO.PcOrCreature = crt;
                    newMO.rank = roll;
                    moveOrderList.Add(newMO);
               
            }
            initialMoveOrderListSize = moveOrderList.Count;
            //sort PCs and creatures based on results
            moveOrderList = moveOrderList.OrderByDescending(x => x.rank).ToList();
            //assign moveOrder to PC and Creature property
            int cnt = 0;
            foreach (MoveOrder m in moveOrderList)
            {
                if (m.PcOrCreature is Player)
                {
                    Player pc = (Player)m.PcOrCreature;
                    pc.moveOrder = cnt;
                }
                else
                {
                    Creature crt = (Creature)m.PcOrCreature;
                    crt.moveOrder = cnt;
                }
                cnt++;
            }
        }
        /*
        public void recalcualteMoveOrder()
        {
            moveOrderList.Clear();
            //creatureCounter2 = 0;
            //go through each PC and creature and make initiative roll
            foreach (Player pc in mod.playerList)
            {
                if (pc.hp > 0)
                {
                    int roll = gv.sf.RandInt(100) + (((pc.dexterity - 10) / 2) * 5);
                    MoveOrder newMO = new MoveOrder();
                    newMO.PcOrCreature = pc;
                    newMO.rank = roll;
                    moveOrderList.Add(newMO);
                }

            }
            foreach (Creature crt in mod.currentEncounter.encounterCreatureList)
            {
                if (crt.hp > 0)
                {
                    int roll = gv.sf.RandInt(100) + (crt.initiativeBonus * 5);
                    MoveOrder newMO = new MoveOrder();
                    newMO.PcOrCreature = crt;
                    newMO.rank = roll;
                    moveOrderList.Add(newMO);
                }
            }
            initialMoveOrderListSize = moveOrderList.Count;
            //sort PCs and creatures based on results
            moveOrderList = moveOrderList.OrderByDescending(x => x.rank).ToList();
            //assign moveOrder to PC and Creature property
            int cnt = 0;
            foreach (MoveOrder m in moveOrderList)
            {
                if (m.PcOrCreature is Player)
                {
                    Player pc = (Player)m.PcOrCreature;
                    pc.moveOrder = cnt;
                }
                else
                {
                    Creature crt = (Creature)m.PcOrCreature;
                    crt.moveOrder = cnt;
                }
                cnt++;
            }
        }
        */
        public void turnController()
        {
            recalculateCreaturesShownInInitiativeBar();
            attackAnimationFrameCounter = 0;
            //redraw screen
            //KArl
            //gv.Render();
            if (currentMoveOrderIndex >= initialMoveOrderListSize)
            {
                //hit the end so start the next round
                startNextRoundStuff();
                return;
            }
            //get the next PC or Creature based on currentMoveOrderIndex and moveOrder property
            int idx = 0;
            foreach (Player pc in mod.playerList)
            {
                if (pc.moveOrder == currentMoveOrderIndex)
                {
                    //highlight the portrait of the pc whose current turn it is
                    gv.cc.ptrPc0.glowOn = false;
                    gv.cc.ptrPc1.glowOn = false;
                    gv.cc.ptrPc2.glowOn = false;
                    gv.cc.ptrPc3.glowOn = false;
                    gv.cc.ptrPc4.glowOn = false;
                    gv.cc.ptrPc5.glowOn = false;
            
                    if (idx == 0) 
                    { 
                        gv.cc.ptrPc0.glowOn = true;
                    }
                    if (idx == 1)
                    {   gv.cc.ptrPc1.glowOn = true; 
                    }
                    if (idx == 2)
                    { 
                        gv.cc.ptrPc2.glowOn = true; 
                    }
                    if (idx == 3)
                    { 
                        gv.cc.ptrPc3.glowOn = true; 
                    }
                    if (idx == 4)
                    { 
                        gv.cc.ptrPc4.glowOn = true; 
                    }
                    if (idx == 5)
                    { 
                        gv.cc.ptrPc5.glowOn = true; 
                    }

                    //write the pc's name to log whsoe turn it is
                    gv.cc.addLogText("<font color='blue'>It's the turn of " + pc.name + ". </font><BR>");

                    //change creatureIndex or currentPlayerIndex
                    currentPlayerIndex = idx;
                    //set isPlayerTurn 
                    isPlayerTurn = true;
                    
                    currentCombatMode = "info";
                    currentMoveOrderIndex++;
                    gv.mod.enteredFirstTime = false;
                    //Karl
                    //gv.Render();
                    //go to start PlayerTurn or start CreatureTurn
                    if ((pc.isHeld()) || (pc.isDead()))
                    {
                        endPcTurn(false);
                    }
                    else
                    {
                        startPcTurn();
                    }
                    return;
                }
                idx++;
            }
            idx = 0;
            foreach (Creature crt in mod.currentEncounter.encounterCreatureList)
            {
                if (crt.moveOrder == currentMoveOrderIndex)
                {
                    coordinatesOfPcTheCreatureMovesTowards.X = -1;
                    coordinatesOfPcTheCreatureMovesTowards.Y = -1;
                    storedPathOfCurrentCreature.Clear();
                    gv.cc.addLogText("<font color='blue'>It's the turn of " + crt.cr_name + ". </font><BR>");
                    //change creatureIndex or currentPlayerIndex
                    creatureIndex = idx;
                    //set isPlayerTurn
                    isPlayerTurn = false;

                    if (!gv.mod.useManualCombatCam)
                    {
                        gv.touchEnabled = false;
                    }

                    currentCombatMode = "info";
                    currentMoveOrderIndex++;
                    //Karl
                    //gv.Render();
                    //go to start PlayerTurn or start CreatureTurn
                    if ((crt.hp > 0) && (!crt.isHeld()))
                    {
                        doCreatureTurn();
                    }
                    else
                    {
                        endCreatureTurn();
                    }                    
                    return;
                }
                idx++;
            }            
            //didn't find one so increment moveOrderIndex and try again
            currentMoveOrderIndex++;
            turnController();
        }        
        public void startNextRoundStuff()
        {
            currentMoveOrderIndex = 0;
            gv.sf.dsWorldTime();
            doHardToKillTrait();
            doBattleRegenTrait();
            foreach (Player pc in mod.playerList)
            {
                RunAllItemCombatRegenerations(pc);
            }
            applyEffectsCombat();
            //IBScript Start Combat Round Hook
            gv.cc.doIBScriptBasedOnFilename(gv.mod.currentEncounter.OnStartCombatRoundIBScript, gv.mod.currentEncounter.OnStartCombatRoundIBScriptParms);
            turnController();
        }
        public void doBattleRegenTrait()
        {
            foreach (Player pc in mod.playerList)
            {
                if (gv.sf.hasTrait(pc, "battleregen"))
                {
                    if (pc.hp <= -20)
                    {
                        //MessageBox("Can't heal a dead character!");
                        gv.cc.addLogText("<font color='red'>" + "BattleRegen off for dead character!" + "</font>" +
                                "<BR>");
                    }
                    else
                    {
                        pc.hp += 1;
                        if (pc.hp > pc.hpMax)
                        {
                            pc.hp = pc.hpMax;
                        }
                        if ((pc.hp > 0) && (pc.charStatus.Equals("Dead")))
                        {
                            pc.charStatus = "Alive";
                        }
                        gv.cc.addLogText("<font color='lime'>" + pc.name + " gains 1 HPs (BattleRegen Trait)" + "</font><BR>");
                    }
                }
            }
        }
        public void doHardToKillTrait()
        {
            foreach (Player pc in mod.playerList)
            {
                if (gv.sf.hasTrait(pc, "hardtokill"))
                {
                    //hard to kill
                    if (pc.hp < 0)
                    {
                        //50% chance to jump back up to 1/4 hpMax
                        int roll = gv.sf.RandInt(100);
                        if (roll > 50)
                        {
                            pc.charStatus = "Alive";
                            pc.hp = pc.hpMax / 10;
                            //do damage to all
                            gv.cc.addLogText("<font color='lime'>" + pc.name + " jumps back up (Hard to Kill trait).</font><br>");
                            if (mod.debugMode)
                            {
                                gv.cc.addLogText("<font color='yellow'>" + "roll = " + roll + " (" + roll + " > 50)</font><BR>");
                            }
                        }
                        else
                        {
                            gv.cc.addLogText("<font color='lime'>" + pc.name + " stays down (Hard to Kill trait).</font><br>");
                            if (mod.debugMode)
                            {
                                gv.cc.addLogText("<font color='yellow'>" + "roll = " + roll + " (" + roll + " < 51)</font><BR>");
                            }
                        }
                    }
                }
            }
        }
        public void RunAllItemCombatRegenerations(Player pc)
        {
            try
            {
                if (mod.getItemByResRefForInfo(pc.BodyRefs.resref).spRegenPerRoundInCombat > 0)
                {
                    doRegenSp(pc, mod.getItemByResRefForInfo(pc.BodyRefs.resref).spRegenPerRoundInCombat);
                }
                if (mod.getItemByResRefForInfo(pc.BodyRefs.resref).hpRegenPerRoundInCombat > 0)
                {
                    doRegenHp(pc, mod.getItemByResRefForInfo(pc.BodyRefs.resref).hpRegenPerRoundInCombat);
                }

                if (mod.getItemByResRefForInfo(pc.MainHandRefs.resref).spRegenPerRoundInCombat > 0)
                {
                    doRegenSp(pc, mod.getItemByResRefForInfo(pc.MainHandRefs.resref).spRegenPerRoundInCombat);
                }
                if (mod.getItemByResRefForInfo(pc.MainHandRefs.resref).hpRegenPerRoundInCombat > 0)
                {
                    doRegenHp(pc, mod.getItemByResRefForInfo(pc.MainHandRefs.resref).hpRegenPerRoundInCombat);
                }

                if (mod.getItemByResRefForInfo(pc.OffHandRefs.resref).spRegenPerRoundInCombat > 0)
                {
                    doRegenSp(pc, mod.getItemByResRefForInfo(pc.OffHandRefs.resref).spRegenPerRoundInCombat);
                }
                if (mod.getItemByResRefForInfo(pc.OffHandRefs.resref).hpRegenPerRoundInCombat > 0)
                {
                    doRegenHp(pc, mod.getItemByResRefForInfo(pc.OffHandRefs.resref).hpRegenPerRoundInCombat);
                }

                if (mod.getItemByResRefForInfo(pc.RingRefs.resref).spRegenPerRoundInCombat > 0)
                {
                    doRegenSp(pc, mod.getItemByResRefForInfo(pc.RingRefs.resref).spRegenPerRoundInCombat);
                }
                if (mod.getItemByResRefForInfo(pc.RingRefs.resref).hpRegenPerRoundInCombat > 0)
                {
                    doRegenHp(pc, mod.getItemByResRefForInfo(pc.RingRefs.resref).hpRegenPerRoundInCombat);
                }

                if (mod.getItemByResRefForInfo(pc.HeadRefs.resref).spRegenPerRoundInCombat > 0)
                {
                    doRegenSp(pc, mod.getItemByResRefForInfo(pc.HeadRefs.resref).spRegenPerRoundInCombat);
                }
                if (mod.getItemByResRefForInfo(pc.HeadRefs.resref).hpRegenPerRoundInCombat > 0)
                {
                    doRegenHp(pc, mod.getItemByResRefForInfo(pc.HeadRefs.resref).hpRegenPerRoundInCombat);
                }

                if (mod.getItemByResRefForInfo(pc.NeckRefs.resref).spRegenPerRoundInCombat > 0)
                {
                    doRegenSp(pc, mod.getItemByResRefForInfo(pc.NeckRefs.resref).spRegenPerRoundInCombat);
                }
                if (mod.getItemByResRefForInfo(pc.NeckRefs.resref).hpRegenPerRoundInCombat > 0)
                {
                    doRegenHp(pc, mod.getItemByResRefForInfo(pc.NeckRefs.resref).hpRegenPerRoundInCombat);
                }

                if (mod.getItemByResRefForInfo(pc.FeetRefs.resref).spRegenPerRoundInCombat > 0)
                {
                    doRegenSp(pc, mod.getItemByResRefForInfo(pc.FeetRefs.resref).spRegenPerRoundInCombat);
                }
                if (mod.getItemByResRefForInfo(pc.FeetRefs.resref).hpRegenPerRoundInCombat > 0)
                {
                    doRegenHp(pc, mod.getItemByResRefForInfo(pc.FeetRefs.resref).hpRegenPerRoundInCombat);
                }

                if (mod.getItemByResRefForInfo(pc.Ring2Refs.resref).spRegenPerRoundInCombat > 0)
                {
                    doRegenSp(pc, mod.getItemByResRefForInfo(pc.Ring2Refs.resref).spRegenPerRoundInCombat);
                }
                if (mod.getItemByResRefForInfo(pc.Ring2Refs.resref).hpRegenPerRoundInCombat > 0)
                {
                    doRegenHp(pc, mod.getItemByResRefForInfo(pc.Ring2Refs.resref).hpRegenPerRoundInCombat);
                }

                if (mod.getItemByResRefForInfo(pc.AmmoRefs.resref).spRegenPerRoundInCombat > 0)
                {
                    doRegenSp(pc, mod.getItemByResRefForInfo(pc.AmmoRefs.resref).spRegenPerRoundInCombat);
                }
                if (mod.getItemByResRefForInfo(pc.AmmoRefs.resref).hpRegenPerRoundInCombat > 0)
                {
                    doRegenHp(pc, mod.getItemByResRefForInfo(pc.AmmoRefs.resref).hpRegenPerRoundInCombat);
                }
            }
            catch (Exception ex)
            {
                gv.errorLog(ex.ToString());
            }
        }
        public void doRegenSp(Player pc, int increment)
        {
            pc.sp += increment;
            if (pc.sp > pc.spMax) { pc.sp = pc.spMax; }
            gv.cc.addLogText("<font color='lime'>" + pc.name + " regens " + increment + "sp</font><br>");

        }
        public void doRegenHp(Player pc, int increment)
        {
            pc.hp += increment;
            if (pc.hp > pc.hpMax) { pc.hp = pc.hpMax; }
            gv.cc.addLogText("<font color='lime'>" + pc.name + " regens " + increment + "hp</font><br>");
        }
        public void applyEffectsCombat()
        {
            try
            {
                //maybe reorder all based on their order property            
                foreach (Player pc in mod.playerList)
                {
                    foreach (Effect ef in pc.effectsList)
                    {
                        //decrement duration of all
                        ef.durationInUnits -= gv.mod.TimePerRound;
                        if (!ef.usedForUpdateStats) //not used for stat updates
                        {
                            gv.cc.doEffectScript(pc, ef);
                        }
                    }
                }
                foreach (Creature crtr in mod.currentEncounter.encounterCreatureList)
                {
                    foreach (Effect ef in crtr.cr_effectsList)
                    {
                        //increment duration of all
                        ef.durationInUnits -= gv.mod.TimePerRound;
                        if (!ef.usedForUpdateStats) //not used for stat updates
                        {
                            //do script for each effect
                            gv.cc.doEffectScript(crtr, ef);
                        }
                    }
                }
                //if remaining duration <= 0, remove from list
                foreach (Player pc in mod.playerList)
                {
                    for (int i = pc.effectsList.Count; i > 0; i--)
                    {
                        if (pc.effectsList[i - 1].durationInUnits <= 0)
                        {
                            pc.effectsList.RemoveAt(i - 1);
                        }
                    }
                }
                foreach (Creature crtr in mod.currentEncounter.encounterCreatureList)
                {
                    for (int i = crtr.cr_effectsList.Count; i > 0; i--)
                    {
                        if (crtr.cr_effectsList[i - 1].durationInUnits <= 0)
                        {
                            crtr.cr_effectsList.RemoveAt(i - 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                IBMessageBox.Show(gv, ex.ToString());
                gv.errorLog(ex.ToString());
            }
            checkEndEncounter();
        }

        //COMBAT	
        #region PC Combat Stuff
        public void decrementAmmo(Player pc)
        {
            if ((mod.getItemByResRefForInfo(pc.MainHandRefs.resref).category.Equals("Ranged"))
                    && (!mod.getItemByResRefForInfo(pc.AmmoRefs.resref).name.Equals("none")))
            {
                ItemRefs itr = mod.getItemRefsInInventoryByResRef(pc.AmmoRefs.resref);
                if (itr != null)
                {
                    //decrement by one
                    itr.quantity--;
                    if (gv.sf.hasTrait(pc, "rapidshot"))
                    {
                        itr.quantity--;
                    }
                    if (gv.sf.hasTrait(pc, "rapidshot2"))
                    {
                        itr.quantity--;
                    }
                    //if equal to zero, remove from party inventory and from all PCs ammo slot
                    if (itr.quantity < 1)
                    {
                        foreach (Player p in mod.playerList)
                        {
                            if (p.AmmoRefs.resref.Equals(itr.resref))
                            {
                                p.AmmoRefs = new ItemRefs();
                            }
                        }
                        mod.partyInventoryRefsList.Remove(itr);
                    }
                }
            }
        }
        public void startPcTurn()
        {
            CalculateUpperLeft();
            //karl
            //gv.Render();
            isPlayerTurn = true;
            gv.touchEnabled = true;
            currentCombatMode = "move";
            Player pc = mod.playerList[currentPlayerIndex];
            gv.sf.UpdateStats(pc);
            currentMoves = 0;
            //do onTurn IBScript
            gv.cc.doIBScriptBasedOnFilename(gv.mod.currentEncounter.OnStartCombatTurnIBScript, gv.mod.currentEncounter.OnStartCombatTurnIBScriptParms);
                        
            if ((pc.isHeld()) || (pc.isDead()) || (pc.isUnconcious()))
            {
                endPcTurn(false);
            }
            if (pc.isImmobile())
            {
                currentMoves = 99;
            }
        }
        public void doCombatAttack(Player pc)
        {
            if (isInRange(pc))
            {
                
                Item itChk = mod.getItemByResRefForInfo(pc.MainHandRefs.resref);
                if (itChk != null)
                {
                    if (itChk.automaticallyHitsTarget) //if AoE type attack and automatically hits
                    {
                        //if using ranged and have ammo, use ammo properties
                        if ((mod.getItemByResRefForInfo(pc.MainHandRefs.resref).category.Equals("Ranged"))
                        && (!mod.getItemByResRefForInfo(pc.AmmoRefs.resref).name.Equals("none")))
                        {
                            itChk = mod.getItemByResRefForInfo(pc.AmmoRefs.resref);
                            if (itChk != null)
                            {
                                //always decrement ammo by one whether a hit or miss
                                this.decrementAmmo(pc);

                                if (!itChk.onScoringHitCastSpellTag.Equals("none"))
                                {
                                    doItemOnHitCastSpell(itChk.onScoringHitCastSpellTag, itChk, targetHighlightCenterLocation);
                                }
                            }
                        }
                        else if (!itChk.onScoringHitCastSpellTag.Equals("none"))
                        {
                            doItemOnHitCastSpell(itChk.onScoringHitCastSpellTag, itChk, targetHighlightCenterLocation);
                        }
                        
                        hitAnimationLocation = new Coordinate(getPixelLocX(targetHighlightCenterLocation.X), getPixelLocY(targetHighlightCenterLocation.Y));

                        //new system
                        AnimationStackGroup newGroup = new AnimationStackGroup();
                        animationSeqStack[0].AnimationSeq.Add(newGroup);
                        addHitAnimation(newGroup);
                        return;
                    }
                }
                

                foreach (Creature crt in mod.currentEncounter.encounterCreatureList)
                {
                    if ((crt.combatLocX == targetHighlightCenterLocation.X) && (crt.combatLocY == targetHighlightCenterLocation.Y))
                    {
                        int attResult = 0; //0=missed, 1=hit, 2=killed
                        int numAtt = 1;
                        int crtLocX = crt.combatLocX;
                        int crtLocY = crt.combatLocY;

                        if ((gv.sf.hasTrait(pc, "twoAttack")) && (mod.getItemByResRefForInfo(pc.MainHandRefs.resref).category.Equals("Melee")))
                        {
                            numAtt = 2;
                        }
                        if ((gv.sf.hasTrait(pc, "rapidshot")) && (mod.getItemByResRefForInfo(pc.MainHandRefs.resref).category.Equals("Ranged")))
                        {
                            numAtt = 2;
                        }
                        if ((gv.sf.hasTrait(pc, "rapidshot2")) && (mod.getItemByResRefForInfo(pc.MainHandRefs.resref).category.Equals("Ranged")))
                        {
                            numAtt = 3;
                        }
                        for (int i = 0; i < numAtt; i++)
                        {
                            if ((gv.sf.hasTrait(pc, "cleave")) && (mod.getItemByResRefForInfo(pc.MainHandRefs.resref).category.Equals("Melee")))
                            {
                                attResult = doActualCombatAttack(pc, crt, i);
                                if (attResult == 2) //2=killed, 1=hit, 0=missed
                                {
                                    Creature crt2 = GetNextAdjacentCreature(pc);
                                    if (crt2 != null)
                                    {
                                        crtLocX = crt2.combatLocX;
                                        crtLocY = crt2.combatLocY;
                                        gv.cc.addFloatyText(new Coordinate(pc.combatLocX, pc.combatLocY), "cleave", "green");
                                        attResult = doActualCombatAttack(pc, crt2, i);
                                    }
                                    break; //do not try and attack same creature that was just killed
                                }
                            }
                            else
                            {
                                attResult = doActualCombatAttack(pc, crt, i);
                                if (attResult == 2) //2=killed, 1=hit, 0=missed
                                {
                                    break; //do not try and attack same creature that was just killed
                                }
                            }

                        }
                        if (attResult > 0) //2=killed, 1=hit, 0=missed
                        {
                            hitAnimationLocation = new Coordinate(getPixelLocX(crtLocX), getPixelLocY(crtLocY));
                            //new system
                            AnimationStackGroup newGroup = new AnimationStackGroup();
                            animationSeqStack[0].AnimationSeq.Add(newGroup);
                            addHitAnimation(newGroup);
                        }
                        else
                        {
                            hitAnimationLocation = new Coordinate(getPixelLocX(crtLocX), getPixelLocY(crtLocY));
                            //new system
                            AnimationStackGroup newGroup = new AnimationStackGroup();
                            animationSeqStack[0].AnimationSeq.Add(newGroup);
                            addMissAnimation(newGroup);
                        }
                        return;
                    }
                }
            }
        }
        public int doActualCombatAttack(Player pc, Creature crt, int attackNumber)
        {
            //always decrement ammo by one whether a hit or miss
            this.decrementAmmo(pc);

            int attackRoll = gv.sf.RandInt(20);
            int attackMod = CalcPcAttackModifier(pc, crt);
            int attack = attackRoll + attackMod;
            int defense = CalcCreatureDefense(pc, crt);
            int damage = CalcPcDamageToCreature(pc, crt);

            bool automaticallyHits = false;
            Item itChk = mod.getItemByResRefForInfo(pc.MainHandRefs.resref);
            if (itChk != null)
            {
                automaticallyHits = itChk.automaticallyHitsTarget;
            }
            //natural 20 always hits
            if ((attack >= defense) || (attackRoll == 20) || (automaticallyHits == true)) //HIT
            {
                crt.hp = crt.hp - damage;
                gv.cc.addLogText("<font color='aqua'>" + pc.name + "</font><font color='white'> attacks </font><font color='silver'>" + crt.cr_name + "</font>");
                gv.cc.addLogText("<font color='white'> and HITS (</font><font color='lime'>" + damage + "</font><font color='white'> damage)</font><BR>");
                gv.cc.addLogText("<font color='white'>" + attackRoll + " + " + attackMod + " >= " + defense + "</font><BR>");

                Item it = mod.getItemByResRefForInfo(pc.MainHandRefs.resref);
                if (it != null)
                {                    
                    doOnHitScriptBasedOnFilename(it.onScoringHit, crt, pc);
                    if (!it.onScoringHitCastSpellTag.Equals("none"))
                    {
                        doItemOnHitCastSpell(it.onScoringHitCastSpellTag, it, crt);
                    }
                }

                it = mod.getItemByResRefForInfo(pc.AmmoRefs.resref);
                if (it != null)
                {
                    doOnHitScriptBasedOnFilename(it.onScoringHit, crt, pc);
                    if (!it.onScoringHitCastSpellTag.Equals("none"))
                    {
                        doItemOnHitCastSpell(it.onScoringHitCastSpellTag, it, crt);
                    }
                }
                                
                //play attack sound for melee (not ranged)
                if (mod.getItemByResRefForInfo(pc.MainHandRefs.resref).category.Equals("Melee"))
                {
                    gv.PlaySound(mod.getItemByResRefForInfo(pc.MainHandRefs.resref).itemOnUseSound);
                }

                //Draw floaty text showing damage above Creature
                int txtH = (int)gv.drawFontRegHeight;
                int shiftUp = 0 - (attackNumber * txtH);
                gv.cc.addFloatyText(new Coordinate(crt.combatLocX, crt.combatLocY), damage + "", shiftUp);

                if (crt.hp <= 0)
                {
                    deathAnimationLocations.Add(new Coordinate(crt.combatLocX, crt.combatLocY));
                    gv.cc.addLogText("<font color='lime'>You killed the " + crt.cr_name + "</font><BR>");
                    return 2; //killed
                }
                else
                {
                    return 1; //hit
                }
            }
            else //MISSED
            {
                //play attack sound for melee (not ranged)
                if (mod.getItemByResRefForInfo(pc.MainHandRefs.resref).category.Equals("Melee"))
                {
                    gv.PlaySound(mod.getItemByResRefForInfo(pc.MainHandRefs.resref).itemOnUseSound);
                }
                gv.cc.addLogText("<font color='aqua'>" + pc.name + "</font><font color='white'> attacks </font><font color='gray'>" + crt.cr_name + "</font>");
                gv.cc.addLogText("<font color='white'> and MISSES</font><BR>");
                gv.cc.addLogText("<font color='white'>" + attackRoll + " + " + attackMod + " < " + defense + "</font><BR>");
                return 0; //missed
            }
        }
        public void doItemOnHitCastSpell(string tag, Item it, object trg)
        {
            Spell sp = gv.mod.getSpellByTag(tag);
            if (sp == null) { return; }
            gv.cc.doSpellBasedOnScriptOrEffectTag(sp, it, trg);
        }        
        public void endPcTurn(bool endStealthMode)
        {
            //KArl
            //gv.Render();
            //remove stealth if endStealthMode = true		
            Player pc = mod.playerList[currentPlayerIndex];
            if (endStealthMode)
            {
                pc.steathModeOn = false;
            }
            else //else test to see if enter/stay in stealth mode if has trait
            {
                doStealthModeCheck(pc);
            }
            canMove = true;
            turnController();
        }
        public void doStealthModeCheck(Player pc)
        {
            int skillMod = 0;
            if (pc.knownTraitsTags.Contains("stealth4"))
            {
                Trait tr = mod.getTraitByTag("stealth4");
                skillMod = tr.skillModifier;
            }
            else if (pc.knownTraitsTags.Contains("stealth3"))
            {
                Trait tr = mod.getTraitByTag("stealth3");
                skillMod = tr.skillModifier;
            }
            else if (pc.knownTraitsTags.Contains("stealth2"))
            {
                Trait tr = mod.getTraitByTag("stealth2");
                skillMod = tr.skillModifier;
            }
            else if (pc.knownTraitsTags.Contains("stealth"))
            {
                Trait tr = mod.getTraitByTag("stealth");
                skillMod = tr.skillModifier;
            }
            else
            {
                //PC doesn't have stealth trait
                pc.steathModeOn = false;
                return;
            }
            int attMod = (pc.dexterity - 10) / 2;
            int roll = gv.sf.RandInt(20);
            int DC = 18; //eventually change to include area modifiers, proximity to enemies, etc.
            if (roll + attMod + skillMod >= DC)
            {
                pc.steathModeOn = true;
                gv.cc.addLogText("<font color='lime'> stealth ON: " + roll + "+" + attMod + "+" + skillMod + ">=" + DC + "</font><BR>");
            }
            else
            {
                pc.steathModeOn = false;
                gv.cc.addLogText("<font color='lime'> stealth OFF: " + roll + "+" + attMod + "+" + skillMod + "<" + DC + "</font><BR>");
            }
        }
        public void doPlayerCombatFacing(Player pc, int tarX, int tarY)
        {
            if ((tarX == pc.combatLocX) && (tarY > pc.combatLocY)) { pc.combatFacing = 2; }
            if ((tarX > pc.combatLocX) && (tarY > pc.combatLocY)) { pc.combatFacing = 3; }
            if ((tarX < pc.combatLocX) && (tarY > pc.combatLocY)) { pc.combatFacing = 1; }
            if ((tarX == pc.combatLocX) && (tarY < pc.combatLocY)) { pc.combatFacing = 8; }
            if ((tarX > pc.combatLocX) && (tarY < pc.combatLocY)) { pc.combatFacing = 9; }
            if ((tarX < pc.combatLocX) && (tarY < pc.combatLocY)) { pc.combatFacing = 7; }
            if ((tarX > pc.combatLocX) && (tarY == pc.combatLocY)) { pc.combatFacing = 6; }
            if ((tarX < pc.combatLocX) && (tarY == pc.combatLocY)) { pc.combatFacing = 4; }
        }
        #endregion

        #region Creature Combat Stuff
	    public void doCreatureTurn()
	    {
		    canMove = true;
		    Creature crt = mod.currentEncounter.encounterCreatureList[creatureIndex];            
		    //do onStartTurn IBScript
            gv.cc.doIBScriptBasedOnFilename(gv.mod.currentEncounter.OnStartCombatTurnIBScript, gv.mod.currentEncounter.OnStartCombatTurnIBScriptParms);
            creatureMoves = 0;
            doCreatureNextAction();
	    }
        public void doCreatureNextAction()
        {
            Creature crt = mod.currentEncounter.encounterCreatureList[creatureIndex];
            CalculateUpperLeftCreature();
            if ((crt.hp > 0) && (!crt.isHeld()))
            {
                creatureToAnimate = null;
                playerToAnimate = null;
                //Karl
                //gv.Render();
                animationState = AnimationState.CreatureThink;
                if (!gv.mod.useManualCombatCam)
                {
                    gv.postDelayed("doAnimation", (int)(2.5f * mod.combatAnimationSpeed));
                }
                else
                {
                    if (((crt.combatLocX + 1) <= (UpperLeftSquare.X + (gv.playerOffsetX * 2))) && ((crt.combatLocX - 1) >= (UpperLeftSquare.X)) && ((crt.combatLocY + 1) <= (UpperLeftSquare.Y + (gv.playerOffsetY * 2))) && ((crt.combatLocY - 1) >= (UpperLeftSquare.Y)))
                    {
                        gv.postDelayed("doAnimation", (int)(2.5f * mod.combatAnimationSpeed));
                        //doCreatureTurnAfterDelay();
                    }
                    else
                    {
                        //gv.postDelayed("doAnimation", 1);
                        doCreatureTurnAfterDelay();
                    }
                }

            }
            else
            { 
                endCreatureTurn();
            }
        }
	    public void doCreatureTurnAfterDelay()
	    {
		    Creature crt = mod.currentEncounter.encounterCreatureList[creatureIndex];
		
		    gv.sf.ActionToTake = null;
		    gv.sf.SpellToCast = null;

            if (crt.isImmobile())
            {
                creatureMoves = 99;
            }

            //determine the action to take
            doCreatureAI(crt);

            //do the action (melee/ranged, cast spell, use trait, etc.)
            if (gv.sf.ActionToTake == null)
            {
        	    endCreatureTurn();
            }
            if (gv.sf.ActionToTake.Equals("Attack"))
            {
                Player pc = targetClosestPC(crt);
        	    gv.sf.CombatTarget = pc;	                
                CreatureDoesAttack(crt);
            }
            else if (gv.sf.ActionToTake.Equals("Move"))
            {
                if ((creatureMoves + 0.5f) < crt.moveDistance)
                {
                    CreatureMoves();
                }
                else
                {
                    endCreatureTurn();
                }
            }
            else if (gv.sf.ActionToTake.Equals("Cast"))
            {
                if ((gv.sf.SpellToCast != null) && (gv.sf.CombatTarget != null))
                {
                    CreatureCastsSpell(crt);
                }
            }
	    }
	    public void CreatureMoves()
	    {
            Creature crt = mod.currentEncounter.encounterCreatureList[creatureIndex];
            if (creatureMoves + 0.5f < crt.moveDistance)
		    {
			    Player pc = targetClosestPC(crt);
                Coordinate newCoor = new Coordinate(-1,-1);
                if (pc != null)
                {
                    if ((pc.combatLocX != coordinatesOfPcTheCreatureMovesTowards.X) || (pc.combatLocY != coordinatesOfPcTheCreatureMovesTowards.Y))
                    {
                        coordinatesOfPcTheCreatureMovesTowards.X = pc.combatLocX;
                        coordinatesOfPcTheCreatureMovesTowards.Y = pc.combatLocY;
                        //run pathFinder to get new location
                        //hurgh200
                        pf.resetGrid();
                        storedPathOfCurrentCreature.Clear();
                        storedPathOfCurrentCreature = pf.findNewPoint(crt, new Coordinate(coordinatesOfPcTheCreatureMovesTowards.X, coordinatesOfPcTheCreatureMovesTowards.Y));
                    }
                }
                if (storedPathOfCurrentCreature.Count > 1)
                {
                    newCoor = storedPathOfCurrentCreature[storedPathOfCurrentCreature.Count - 2];
                }
                //remove one from list upon move... to do

                //hurgh100
                if (pc != null)
			    {
				    //pf.resetGrid();
				    //newCoor = pf.findNewPoint(crt, new Coordinate(pc.combatLocX, pc.combatLocY));
				    if ((newCoor.X == -1) && (newCoor.Y == -1))
				    {
					    //didn't find a path, don't move
                        //KArl
                        //gv.Render();
			    	    endCreatureTurn();
					    return;
				    }
				   
                    //it's a diagonal move
                    if ((crt.combatLocX != newCoor.X) && (crt.combatLocY != newCoor.Y))
                    {
                        //enough  move points availbale to do the diagonal move
                        if ((crt.moveDistance - creatureMoves) >= mod.diagonalMoveCost)
                        {
                            if ((newCoor.X < crt.combatLocX) && (!crt.combatFacingLeft)) //move left
                            {
                                crt.combatFacingLeft = true;
                            }
                            else if ((newCoor.X > crt.combatLocX) && (crt.combatFacingLeft)) //move right
                            {
                                crt.combatFacingLeft = false;
                            }
                            //CHANGE FACING BASED ON MOVE
                            doCreatureCombatFacing(crt, newCoor.X, newCoor.Y);
                            moveCost = mod.diagonalMoveCost;
                            crt.combatLocX = newCoor.X;
                            crt.combatLocY = newCoor.Y;
                            if (storedPathOfCurrentCreature.Count > 1)
                            {
                                storedPathOfCurrentCreature.RemoveAt(storedPathOfCurrentCreature.Count - 2);
                            }
                            canMove = false;
                            animationState = AnimationState.CreatureMove;
                            //hurgh20!
                            if (gv.mod.useManualCombatCam)
                            {
                                if (((crt.combatLocX + 1) <= (UpperLeftSquare.X + (gv.playerOffsetX * 2))) && ((crt.combatLocX - 1) >= (UpperLeftSquare.X)) && ((crt.combatLocY + 1) <= (UpperLeftSquare.Y + (gv.playerOffsetY * 2))) && ((crt.combatLocY - 1) >= (UpperLeftSquare.Y)))
                                {
                                    gv.postDelayed("doAnimation", (int)(1f * mod.combatAnimationSpeed));
                                }
                                else
                                {
                                    gv.postDelayed("doAnimation", 1);
                                }
                                
                            }
                            else
                            {
                                gv.postDelayed("doAnimation", (int)(1f * mod.combatAnimationSpeed));
                            }

                        }
                        
                        //try to move horizontally or vertically instead if most points are not enough for diagonal move
                        else if ((crt.moveDistance - creatureMoves) >= 1)
                        {
                            //pf.resetGrid();
                            //block the originial diagonal target square and calculate again
                            mod.nonAllowedDiagonalSquareX = newCoor.X;
                            mod.nonAllowedDiagonalSquareY = newCoor.Y;
                            //newCoor = pf.findNewPoint(crt, new Coordinate(pc.combatLocX, pc.combatLocY));
                            if ((newCoor.X == -1) && (newCoor.Y == -1))
                            {
                                //didn't find a path, don't move
                                //KARL
                                //gv.Render();
                                endCreatureTurn();
                                return;
                            }
                            if ((newCoor.X < crt.combatLocX) && (!crt.combatFacingLeft)) //move left
                            {
                                crt.combatFacingLeft = true;
                            }
                            else if ((newCoor.X > crt.combatLocX) && (crt.combatFacingLeft)) //move right
                            {
                                crt.combatFacingLeft = false;
                            }
                            //CHANGE FACING BASED ON MOVE
                            doCreatureCombatFacing(crt, newCoor.X, newCoor.Y);
                            moveCost = 1;
                            crt.combatLocX = newCoor.X;
                            crt.combatLocY = newCoor.Y;
                            if (storedPathOfCurrentCreature.Count > 1)
                            {
                                storedPathOfCurrentCreature.RemoveAt(storedPathOfCurrentCreature.Count - 2);
                            }
                            canMove = false;
                            animationState = AnimationState.CreatureMove;

                            if (gv.mod.useManualCombatCam)
                            {
                                if (((crt.combatLocX + 1) <= (UpperLeftSquare.X + (gv.playerOffsetX * 2))) && ((crt.combatLocX - 1) >= (UpperLeftSquare.X)) && ((crt.combatLocY + 1) <= (UpperLeftSquare.Y + (gv.playerOffsetY * 2))) && ((crt.combatLocY - 1) >= (UpperLeftSquare.Y)))
                                {
                                    gv.postDelayed("doAnimation", (int)(1f * mod.combatAnimationSpeed));
                                }
                                else
                                {
                                    gv.postDelayed("doAnimation", 1);
                                }
                            }
                            else
                            {
                                gv.postDelayed("doAnimation", (int)(1f * mod.combatAnimationSpeed));
                            }

                        }
                        //less than one move point, no move
                        else
                        {
                            canMove = false;
                            animationState = AnimationState.CreatureMove;
                            if (gv.mod.useManualCombatCam)
                            {
                                if (((crt.combatLocX + 1) <= (UpperLeftSquare.X + (gv.playerOffsetX * 2))) && ((crt.combatLocX - 1) >= (UpperLeftSquare.X)) && ((crt.combatLocY + 1) <= (UpperLeftSquare.Y + (gv.playerOffsetY * 2))) && ((crt.combatLocY - 1) >= (UpperLeftSquare.Y)))
                                {
                                    gv.postDelayed("doAnimation", (int)(1f * mod.combatAnimationSpeed));
                                }
                                else
                                {
                                    gv.postDelayed("doAnimation", 1);
                                }
                            }
                            else
                            {
                                gv.postDelayed("doAnimation", (int)(1f * mod.combatAnimationSpeed));
                            }

                        }
                    }
                   //it's a horizontal or vertical move
                    else
                    {
                        if ((newCoor.X < crt.combatLocX) && (!crt.combatFacingLeft)) //move left
                        {
                            crt.combatFacingLeft = true;
                        }
                        else if ((newCoor.X > crt.combatLocX) && (crt.combatFacingLeft)) //move right
                        {
                            crt.combatFacingLeft = false;
                        }
                        //CHANGE FACING BASED ON MOVE
                        doCreatureCombatFacing(crt, newCoor.X, newCoor.Y);
                        crt.combatLocX = newCoor.X;
                        crt.combatLocY = newCoor.Y;
                        if (storedPathOfCurrentCreature.Count > 1)
                        {
                            storedPathOfCurrentCreature.RemoveAt(storedPathOfCurrentCreature.Count - 2);
                        }
                        canMove = false;
                        animationState = AnimationState.CreatureMove;
                        if (gv.mod.useManualCombatCam)
                        {
                            if (((crt.combatLocX + 1) <= (UpperLeftSquare.X + (gv.playerOffsetX * 2))) && ((crt.combatLocX - 1) >= (UpperLeftSquare.X)) && ((crt.combatLocY + 1) <= (UpperLeftSquare.Y + (gv.playerOffsetY * 2))) && ((crt.combatLocY - 1) >= (UpperLeftSquare.Y)))
                            {
                                gv.postDelayed("doAnimation", (int)(1f * mod.combatAnimationSpeed));
                            }
                            else 
                            {
                                gv.postDelayed("doAnimation", 1);
                            }
                            
                        }
                        else
                        {
                            gv.postDelayed("doAnimation", (int)(1f * mod.combatAnimationSpeed));
                        }

                    }
			    }
			    else //no target found
			    {
                    //KArl
                    //gv.Render();
		    	    endCreatureTurn();
				    return;
			    }
		    }
            //less than a move point left, no move
		    else
		    {
                gv.Render();
	    	    endCreatureTurn();
			    return;
		    }
	    }
	    public void CreatureDoesAttack(Creature crt)
        {
            if (gv.sf.CombatTarget != null)
            {
	            Player pc = (Player)gv.sf.CombatTarget;
                //Uses Map Pixel Locations
	            int endX = pc.combatLocX * gv.squareSize + (gv.squareSize / 2);
			    int endY = pc.combatLocY * gv.squareSize + (gv.squareSize / 2);
			    int startX = crt.combatLocX * gv.squareSize + (gv.squareSize / 2);
			    int startY = crt.combatLocY * gv.squareSize + (gv.squareSize / 2);
	            // determine if ranged or melee
	            if ((crt.cr_category.Equals("Ranged")) 
	        		    && (CalcDistance(crt.combatLocX, crt.combatLocY, pc.combatLocX, pc.combatLocY) <= crt.cr_attRange)
	        		    && (isVisibleLineOfSight(new Coordinate(endX, endY), new Coordinate(startX, startY))))
	            {
	                //play attack sound for ranged
	        	    gv.PlaySound(crt.cr_attackSound);
	                if ((pc.combatLocX < crt.combatLocX) && (!crt.combatFacingLeft)) //attack left
        		    {
        			    crt.combatFacingLeft = true;
        		    }
        		    else if ((pc.combatLocX > crt.combatLocX) && (crt.combatFacingLeft)) //attack right
        		    {
        			    crt.combatFacingLeft = false;
        		    }
                    //CHANGE FACING BASED ON ATTACK
                    doCreatureCombatFacing(crt, pc.combatLocX, pc.combatLocY);

                    if (crt.hp > 0)
	                {

                        if (gv.mod.useManualCombatCam)
                        {
                            adjustCamToRangedCreature = true;
                            CalculateUpperLeftCreature();
                            adjustCamToRangedCreature = false;

                            if (IsInVisibleCombatWindow(crt.combatLocX, crt.combatLocY))
                            {
                                gv.touchEnabled = false;
                            }
                        }

                        creatureToAnimate = crt;
	    	            playerToAnimate = null;
                        creatureTargetLocation = new Coordinate(pc.combatLocX, pc.combatLocY);
                        //set attack animation and do a delay
                        attackAnimationTimeElapsed = 0;
                        attackAnimationLengthInMilliseconds = (int)(5f * mod.combatAnimationSpeed);
                        //add projectile animation
                        startX = getPixelLocX(crt.combatLocX);
                        startY = getPixelLocY(crt.combatLocY);
                        endX = getPixelLocX(pc.combatLocX);
                        endY = getPixelLocY(pc.combatLocY);
                        string filename = crt.cr_projSpriteFilename;
                        AnimationSequence newSeq = new AnimationSequence();
                        animationSeqStack.Add(newSeq);
                        AnimationStackGroup newGroup = new AnimationStackGroup();
                        newSeq.AnimationSeq.Add(newGroup);
                        launchProjectile(filename, startX, startY, endX, endY, newGroup);
                        //add ending projectile animation  
                        doStandardCreatureAttack();
                        //add hit or miss animation
                        //add floaty text
                        //add death animations
                        newGroup = new AnimationStackGroup();
                        animationSeqStack[0].AnimationSeq.Add(newGroup);
                        foreach (Coordinate coor in deathAnimationLocations)
                        {
                            if (!IsInVisibleCombatWindow(coor.X, coor.Y))
                            {
                                continue;
                            }
                            addDeathAnimation(newGroup, new Coordinate(getPixelLocX(coor.X), getPixelLocY(coor.Y)));
                        }
                        animationsOn = true;
                    }
	                else
	                {
	                    //skip this guys turn
	                }
	            }
	            else if ((crt.cr_category.Equals("Melee")) 
	        		    && (CalcDistance(crt.combatLocX, crt.combatLocY, pc.combatLocX, pc.combatLocY) <= crt.cr_attRange))
	            {
	        	    if ((pc.combatLocX < crt.combatLocX) && (!crt.combatFacingLeft)) //attack left
        		    {
        			    crt.combatFacingLeft = true;
        		    }
        		    else if ((pc.combatLocX > crt.combatLocX) && (crt.combatFacingLeft)) //attack right
        		    {
        			    crt.combatFacingLeft = false;
        		    }
                    //CHANGE FACING BASED ON ATTACK
                    doCreatureCombatFacing(crt, pc.combatLocX, pc.combatLocY);
	                if (crt.hp > 0)
	                {
	            	    creatureToAnimate = crt;
	    	            playerToAnimate = null;

                        attackAnimationTimeElapsed = 0;
                        attackAnimationLengthInMilliseconds = (int)(5f * mod.combatAnimationSpeed);


                        //do melee attack stuff and animations  
                        AnimationSequence newSeq = new AnimationSequence();
                        animationSeqStack.Add(newSeq);
                        doStandardCreatureAttack();
                        //add hit or miss animation
                        //add floaty text
                        //add death animations
                        AnimationStackGroup newGroup = new AnimationStackGroup();
                        animationSeqStack[0].AnimationSeq.Add(newGroup);
                        foreach (Coordinate coor in deathAnimationLocations)
                        {
                            if (!IsInVisibleCombatWindow(coor.X, coor.Y))
                            {
                                continue;
                            }
                            addDeathAnimation(newGroup, new Coordinate(getPixelLocX(coor.X), getPixelLocY(coor.Y)));
                        }
                        animationsOn = true;
	                }
	                else
	                {
	                    //skip this guys turn
	                }
	            }
	            else //not in range for attack so MOVE
	            {
	        	    CreatureMoves();
	            }
            }
            else //no target so move instead
            {
        	    CreatureMoves();
            }
        }
        public void CreatureCastsSpell(Creature crt)
        {
            Coordinate pnt = new Coordinate();
            if (gv.sf.CombatTarget is Player)
            {
                Player pc = (Player)gv.sf.CombatTarget;
                pnt = new Coordinate(pc.combatLocX, pc.combatLocY);
            }
            else if (gv.sf.CombatTarget is Creature)
            {
                Creature crtTarget = (Creature)gv.sf.CombatTarget;
                pnt = new Coordinate(crtTarget.combatLocX, crtTarget.combatLocY);
            }
            else if (gv.sf.CombatTarget is Coordinate)
            {
                pnt = (Coordinate)gv.sf.CombatTarget;
            }
            else //do not understand, what is the target
            {
        	    return;
            }
            //Using Map Pixel Locations
            int endX = pnt.X * gv.squareSize + (gv.squareSize / 2);
		    int endY = pnt.Y * gv.squareSize + (gv.squareSize / 2);
		    int startX = crt.combatLocX * gv.squareSize + (gv.squareSize / 2);
		    int startY = crt.combatLocY * gv.squareSize + (gv.squareSize / 2);
            if ((getDistance(pnt, new Coordinate(crt.combatLocX, crt.combatLocY)) <= gv.sf.SpellToCast.range) 
        		    && (isVisibleLineOfSight(new Coordinate(endX, endY), new Coordinate(startX, startY))))
            {

                if (gv.mod.useManualCombatCam)
                {
                    adjustCamToRangedCreature = true;
                    CalculateUpperLeftCreature();
                    adjustCamToRangedCreature = false;

                    if (IsInVisibleCombatWindow(crt.combatLocX, crt.combatLocY))
                    {
                        gv.touchEnabled = false;
                    }
                }

                if ((pnt.X < crt.combatLocX) && (!crt.combatFacingLeft)) //attack left
    		    {
    			    crt.combatFacingLeft = true;
    		    }
    		    else if ((pnt.X > crt.combatLocX) && (crt.combatFacingLeft)) //attack right
    		    {
    			    crt.combatFacingLeft = false;
    		    }
                //CHANGE FACING BASED ON ATTACK
                doCreatureCombatFacing(crt, pnt.X, pnt.Y);
        	    creatureTargetLocation = pnt;
        	    creatureToAnimate = crt;
	            playerToAnimate = null;

                //set attack animation and do a delay
                attackAnimationTimeElapsed = 0;
                attackAnimationLengthInMilliseconds = (int)(5f * mod.combatAnimationSpeed);
                AnimationSequence newSeq = new AnimationSequence();
                animationSeqStack.Add(newSeq);
                //add projectile animation
                gv.PlaySound(gv.sf.SpellToCast.spellStartSound);
                startX = getPixelLocX(crt.combatLocX);
                startY = getPixelLocY(crt.combatLocY);
                endX = getPixelLocX(creatureTargetLocation.X);
                endY = getPixelLocY(creatureTargetLocation.Y);
                string filename = gv.sf.SpellToCast.spriteFilename;
                AnimationStackGroup newGroup = new AnimationStackGroup();
                newSeq.AnimationSeq.Add(newGroup);
                launchProjectile(filename, startX, startY, endX, endY, newGroup);
                //gv.PlaySound(gv.sf.SpellToCast.spellEndSound);
                gv.cc.doSpellBasedOnScriptOrEffectTag(gv.sf.SpellToCast, crt, gv.sf.CombatTarget);
                //add ending projectile animation
                newGroup = new AnimationStackGroup();
                animationSeqStack[0].AnimationSeq.Add(newGroup);
                filename = gv.sf.SpellToCast.spriteEndingFilename;
                foreach (Coordinate coor in gv.sf.AoeSquaresList)
                {
                    if (!IsInVisibleCombatWindow(coor.X, coor.Y))
                    {
                        continue;
                    }
                    addEndingAnimation(newGroup, new Coordinate(getPixelLocX(coor.X), getPixelLocY(coor.Y)), filename);
                }
                //add floaty text
                //add death animations
                newGroup = new AnimationStackGroup();
                animationSeqStack[0].AnimationSeq.Add(newGroup);
                foreach (Coordinate coor in deathAnimationLocations)
                {
                    if (!IsInVisibleCombatWindow(coor.X, coor.Y))
                    {
                        continue;
                    }
                    addDeathAnimation(newGroup, new Coordinate(getPixelLocX(coor.X), getPixelLocY(coor.Y)));
                }
                animationsOn = true;                
            }
            else
            {
                //#region Do a Melee or Ranged Attack
                Player pc = targetClosestPC(crt);
                gv.sf.CombatTarget = pc;	                
                CreatureDoesAttack(crt);
            }
        }
	    public void doCreatureAI(Creature crt)
	    {
		    //These are the current generic AI types
            //BasicAttacker:          basic attack (ranged or melee)
            //Healer:                 heal Friend(s) until out of SP
            //BattleHealer:           heal Friend(s) and/or attack
            //DamageCaster:           cast damage spells
            //BattleDamageCaster:     cast damage spells and/or attack
            //DebuffCaster:           cast debuff spells
            //BattleDebuffCaster:     cast debuff spells and/or attack
            //GeneralCaster:          cast any of their known spells by random
            //BattleGeneralCaster:    cast any of their known spells by random and/or attack

            if (crt.cr_ai.Equals("BasicAttacker"))
            {
                if (gv.mod.debugMode)
                {
                    gv.cc.addLogText("<font color='yellow'>" + crt.cr_name + " is a BasicAttacker</font><BR>");
                }
                BasicAttacker(crt);
            }        
            else if (crt.cr_ai.Equals("GeneralCaster"))
            {
                if (gv.mod.debugMode)
                {
                    gv.cc.addLogText("<font color='yellow'>" + crt.cr_name + " is a GeneralCaster</font><BR>");
                }
                GeneralCaster(crt);
            }        
            else
            {
                if (gv.mod.debugMode)
                {
                    gv.cc.addLogText("<font color='yellow'>" + crt.cr_name + " is a BasicAttacker</font><BR>");
                }
                BasicAttacker(crt);
            }
	    }
	    public void BasicAttacker(Creature crt)
        {
            Player pc = targetClosestPC(crt);
            gv.sf.CombatTarget = pc;
            int dist = CalcDistance(crt.combatLocX, crt.combatLocY, pc.combatLocX, pc.combatLocY);
            if (dist <= crt.cr_attRange)
            {
                gv.sf.ActionToTake = "Attack";
            }
            else
            {
                gv.sf.ActionToTake = "Move";
            }
        }
        public void GeneralCaster(Creature crt)
        {
    	    gv.sf.SpellToCast = null;
            //just pick a random spell from KnownSpells
            //try a few times to pick a random spell that has enough SP
            for (int i = 0; i < 10; i++)
            {
                int rnd = gv.sf.RandInt(crt.knownSpellsTags.Count);
                Spell sp = mod.getSpellByTag(crt.knownSpellsTags[rnd-1]);
                if (sp != null)
                {
                    if (sp.costSP <= crt.sp)
                    {
                	    gv.sf.SpellToCast = sp;
                        
                        if (gv.sf.SpellToCast.spellTargetType.Equals("Enemy"))
                        {
                            Player pc = targetClosestPC(crt);
                            gv.sf.CombatTarget = pc;
                            gv.sf.ActionToTake = "Cast";
                            break;
                        }
                        else if (gv.sf.SpellToCast.spellTargetType.Equals("PointLocation"))
                        {
                    	    Coordinate bestLoc = targetBestPointLocation(crt);
                    	    if (bestLoc == new Coordinate(-1,-1))
                    	    {
                    		    //didn't find a target so use closest PC
                    		    Player pc = targetClosestPC(crt);
                        	    gv.sf.CombatTarget = new Coordinate(pc.combatLocX, pc.combatLocY);
                    	    }
                    	    else
                    	    {
                    		    gv.sf.CombatTarget = targetBestPointLocation(crt);
                    	    }                    	
                    	    gv.sf.ActionToTake = "Cast";
                            break;
                        }
                        else if (gv.sf.SpellToCast.spellTargetType.Equals("Friend"))
                        {
                            //target is another creature (currently assumed that spell is a heal spell)
                            Creature targetCrt = GetCreatureWithMostDamaged();
                            if (targetCrt != null)
                            {
                        	    gv.sf.CombatTarget = targetCrt;
                        	    gv.sf.ActionToTake = "Cast";
                                break;
                            }
                            else //didn't find a target that needs HP
                            {
                        	    gv.sf.SpellToCast = null;
                        	    continue;
                            }
                        }
                        else if (gv.sf.SpellToCast.spellTargetType.Equals("Self"))
                        {
                            //target is self (currently assumed that spell is a heal spell)
                            Creature targetCrt = crt;
                            if (targetCrt != null)
                            {
                        	    gv.sf.CombatTarget = targetCrt;
                        	    gv.sf.ActionToTake = "Cast";
                                break;
                            }
                        }
                        else //didn't find a target so set to null so that will use attack instead
                        {
                    	    gv.sf.SpellToCast = null;
                        }
                    }
                }
            }
            if (gv.sf.SpellToCast == null) //didn't find a spell that matched the criteria so use attack instead
            {
        	    Player pc = targetClosestPC(crt);
        	    gv.sf.CombatTarget = pc;
        	    gv.sf.ActionToTake = "Attack";
            }
        }
        public void endCreatureTurn()
        {
            //KArl
            //gv.Render();

            canMove = true;
            gv.sf.ActionToTake = null;
            gv.sf.SpellToCast = null;
            //coordinatesOfPcTheCreatureMovesTowards.X = -1;
            //coordinatesOfPcTheCreatureMovesTowards.Y = -1;
            if (checkEndEncounter())
            {
                return;
            }
            turnController();
        }
        public void doStandardCreatureAttack()
        {
            Creature crt = mod.currentEncounter.encounterCreatureList[creatureIndex];
            Player pc = (Player)gv.sf.CombatTarget;

            bool hit = false;
            for (int i = 0; i < crt.cr_numberOfAttacks; i++)
            {
                //this reduces the to hit bonus for each further creature attack by an additional -5
                //creatureMultAttackPenalty = 5 * i;            
                bool hitreturn = doActualCreatureAttack(pc, crt, i);
                if (hitreturn) { hit = true; }
                if (pc.hp <= 0)
                {
                    break; //do not try and attack same PC that was just killed
                }
            }

            //play attack sound for melee
            if (!crt.cr_category.Equals("Ranged"))
            {
                gv.PlaySound(crt.cr_attackSound);
            }

            if (hit)
            {
                hitAnimationLocation = new Coordinate(getPixelLocX(pc.combatLocX), getPixelLocY(pc.combatLocY));
                //new system
                AnimationStackGroup newGroup = new AnimationStackGroup();
                animationSeqStack[0].AnimationSeq.Add(newGroup);
                addHitAnimation(newGroup);
            }
            else
            {
                hitAnimationLocation = new Coordinate(getPixelLocX(pc.combatLocX), getPixelLocY(pc.combatLocY));
                //new system
                AnimationStackGroup newGroup = new AnimationStackGroup();
                animationSeqStack[0].AnimationSeq.Add(newGroup);
                addMissAnimation(newGroup);
            }
        }
        public bool doActualCreatureAttack(Player pc, Creature crt, int attackNumber)
        {
            int attackRoll = gv.sf.RandInt(20);
            int attackMod = CalcCreatureAttackModifier(crt, pc);
            int defense = CalcPcDefense(pc, crt);
            int damage = CalcCreatureDamageToPc(pc, crt);
            int attack = attackRoll + attackMod;

            if ((attack >= defense) || (attackRoll == 20))
            {
                pc.hp = pc.hp - damage;
                gv.cc.addLogText("<font color='silver'>" + crt.cr_name + "</font>" +
                        "<font color='white'>" + " attacks " + "</font>" +
                        "<font color='aqua'>" + pc.name + "</font><BR>");
                gv.cc.addLogText("<font color='white'>" + " and HITS (" + "</font>" +
                        "<font color='red'>" + damage + "</font>" +
                        "<font color='white'>" + " damage)" + "</font><BR>");
                gv.cc.addLogText("<font color='white'>" + attackRoll + " + " + attackMod + " >= " + defense + "</font><BR>");

                doOnHitScriptBasedOnFilename(crt.onScoringHit, crt, pc);
                if (!crt.onScoringHitCastSpellTag.Equals("none"))
                {
                    doCreatureOnHitCastSpell(crt, pc);
                }
                
                //Draw floaty text showing damage above PC
                int txtH = (int)gv.drawFontRegHeight;
                int shiftUp = 0 - (attackNumber * txtH);
                gv.cc.addFloatyText(new Coordinate(pc.combatLocX, pc.combatLocY), damage + "", shiftUp);

                if (pc.hp <= 0)
                {
                    gv.cc.addLogText("<font color='red'>" + pc.name + " drops down unconsciously!" + "</font><BR>");
                    pc.charStatus = "Dead";
                }
                if (pc.hp <= -20)
                {
                    deathAnimationLocations.Add(new Coordinate(pc.combatLocX, pc.combatLocY));
                }
                return true;
            }
            else
            {
                gv.cc.addLogText("<font color='silver'>" + crt.cr_name + "</font>" +
                        "<font color='white'>" + " attacks " + "</font>" +
                        "<font color='aqua'>" + pc.name + "</font><BR>");
                gv.cc.addLogText("<font color='white'>" + " and MISSES" + "</font><BR>");
                gv.cc.addLogText("<font color='white'>" + attackRoll + " + " + attackMod + " < " + defense + "</font><BR>");
                return false;
            }
        }
        public void doCreatureCombatFacing(Creature crt, int tarX, int tarY)
        {
            if ((tarX == crt.combatLocX) && (tarY > crt.combatLocY)) { crt.combatFacing = 2; }
            if ((tarX > crt.combatLocX) && (tarY > crt.combatLocY)) { crt.combatFacing = 3; }
            if ((tarX < crt.combatLocX) && (tarY > crt.combatLocY)) { crt.combatFacing = 1; }
            if ((tarX == crt.combatLocX) && (tarY < crt.combatLocY)) { crt.combatFacing = 8; }
            if ((tarX > crt.combatLocX) && (tarY < crt.combatLocY)) { crt.combatFacing = 9; }
            if ((tarX < crt.combatLocX) && (tarY < crt.combatLocY)) { crt.combatFacing = 7; }
            if ((tarX > crt.combatLocX) && (tarY == crt.combatLocY)) { crt.combatFacing = 6; }
            if ((tarX < crt.combatLocX) && (tarY == crt.combatLocY)) { crt.combatFacing = 4; }
        }
        #endregion

        public void doOnHitScriptBasedOnFilename(string filename, Creature crt, Player pc)
        {
            if (!filename.Equals("none"))
            {
                try
                {
                    if (filename.Equals("onHitBeetleFire.cs"))
                    {
                        float resist = (float)(1f - ((float)pc.damageTypeResistanceTotalFire / 100f));
                        float damage = (1 * gv.sf.RandInt(2)) + 0;
                        int fireDam = (int)(damage * resist);

                        if (mod.debugMode)
                        {
                            gv.cc.addLogText("<font color='yellow'>" + "resist = " + resist + " damage = " + damage
                                        + " fireDam = " + fireDam + "</font>" +
                                        "<BR>");
                        }
                        gv.cc.addLogText("<font color='aqua'>" + pc.name + "</font>" +
                                "<font color='white'>" + " is burned for " + "</font>" +
                                "<font color='red'>" + fireDam + "</font>" +
                                "<font color='white'>" + " hit point(s)" + "</font>" +
                                "<BR>");
                        pc.hp -= fireDam;
                    }

                    else if (filename.Equals("onHitMaceOfStunning.cs"))
                    {
                        int tryHold = gv.sf.RandInt(100);
                        if (tryHold > 50)
                        {
                            //attempt to hold PC
                            int saveChkRoll = gv.sf.RandInt(20);
                            int saveChk = saveChkRoll + crt.fortitude;
                            int DC = 15;
                            if (saveChk >= DC) //passed save check
                            {
                                gv.cc.addLogText("<font color='yellow'>" + crt.cr_name + " avoids stun (" + saveChkRoll + " + " + crt.fortitude + " >= " + DC + ")</font><BR>");
                            }
                            else
                            {
                                gv.cc.addLogText("<font color='red'>" + crt.cr_name + " is stunned by mace (" + saveChkRoll + " + " + crt.fortitude + " < " + DC + ")</font><BR>");
                                crt.cr_status = "Held";
                                Effect ef = mod.getEffectByTag("hold");
                                crt.AddEffectByObject(ef, 1);
                            }
                        }
                    }
                    else if (filename.Equals("onHitBeetleAcid.cs"))
                    {
                        float resist = (float)(1f - ((float)pc.damageTypeResistanceTotalAcid / 100f));
                        float damage = (1 * gv.sf.RandInt(2)) + 0;
                        int acidDam = (int)(damage * resist);

                        if (mod.debugMode)
                        {
                            gv.cc.addLogText("<font color='yellow'>" + "resist = " + resist + " damage = " + damage
                                    + " acidDam = " + acidDam + "</font>" +
                                    "<BR>");
                        }
                        gv.cc.addLogText("<font color='aqua'>" + pc.name + "</font>" +
                                "<font color='white'>" + " is burned for " + "</font>" +
                                "<font color='lime'>" + acidDam + "</font>" +
                                "<font color='white'>" + " hit point(s)" + "</font>" +
                                "<BR>");
                        pc.hp -= acidDam;

                        //attempt to hold PC
                        int saveChkRoll = gv.sf.RandInt(20);
                        int saveChk = saveChkRoll + pc.fortitude;
                        int DC = 10;
                        if (saveChk >= DC) //passed save check
                        {
                            gv.cc.addLogText("<font color='yellow'>" + pc.name + " avoids the acid stun (" + saveChkRoll + " + " + pc.fortitude + " >= " + DC + ")</font><BR>");
                        }
                        else
                        {
                            gv.cc.addLogText("<font color='red'>" + pc.name + " is held by an acid stun (" + saveChkRoll + " + " + pc.fortitude + " < " + DC + ")</font><BR>");
                            pc.charStatus = "Held";
                            Effect ef = mod.getEffectByTag("hold");
                            pc.AddEffectByObject(ef, 1);
                        }
                    }
                    else if (filename.Equals("onHitOneFire.cs"))
                    {
                        float resist = (float)(1f - ((float)crt.damageTypeResistanceValueFire / 100f));
                        float damage = 1.0f;
                        int fireDam = (int)(damage * resist);

                        if (mod.debugMode)
                        {
                            gv.cc.addLogText("<font color='yellow'>" + "resist = " + resist + " damage = " + damage
                                        + " fireDam = " + fireDam + "</font>" +
                                        "<BR>");
                        }
                        gv.cc.addLogText("<font color='aqua'>" + crt.cr_name + "</font>" +
                                "<font color='white'>" + " is burned for " + "</font>" +
                                "<font color='red'>" + fireDam + "</font>" +
                                "<font color='white'>" + " hit point(s)" + "</font>" +
                                "<BR>");
                        crt.hp -= fireDam;
                    }
                    else if (filename.Equals("onHitOneTwoFire.cs"))
                    {
                        float resist = (float)(1f - ((float)crt.damageTypeResistanceValueFire / 100f));
                        float damage = (1 * gv.sf.RandInt(2)) + 0;
                        int fireDam = (int)(damage * resist);

                        if (mod.debugMode)
                        {
                            gv.cc.addLogText("<font color='yellow'>" + "resist = " + resist + " damage = " + damage
                                        + " fireDam = " + fireDam + "</font>" +
                                        "<BR>");
                        }
                        gv.cc.addLogText("<font color='aqua'>" + crt.cr_name + "</font>" +
                                "<font color='white'>" + " is burned for " + "</font>" +
                                "<font color='red'>" + fireDam + "</font>" +
                                "<font color='white'>" + " hit point(s)" + "</font>" +
                                "<BR>");
                        crt.hp -= fireDam;
                    }
                    else if (filename.Equals("onHitTwoThreeFire.cs"))
                    {
                        float resist = (float)(1f - ((float)crt.damageTypeResistanceValueFire / 100f));
                        float damage = (1 * gv.sf.RandInt(2)) + 1;
                        int fireDam = (int)(damage * resist);

                        if (mod.debugMode)
                        {
                            gv.cc.addLogText("<font color='yellow'>" + "resist = " + resist + " damage = " + damage
                                        + " fireDam = " + fireDam + "</font>" +
                                        "<BR>");
                        }
                        gv.cc.addLogText("<font color='aqua'>" + crt.cr_name + "</font>" +
                                "<font color='white'>" + " is burned for " + "</font>" +
                                "<font color='red'>" + fireDam + "</font>" +
                                "<font color='white'>" + " hit point(s)" + "</font>" +
                                "<BR>");
                        crt.hp -= fireDam;
                    }
                    else if (filename.Equals("onHitPcPoisonedLight.cs"))
                    {
                        int saveChkRoll = gv.sf.RandInt(20);
                        int saveChk = saveChkRoll + pc.reflex;
                        int DC = 13;
                        if (saveChk >= DC) //passed save check
                        {
                            gv.cc.addLogText("<font color='yellow'>" + pc.name + " avoids being poisoned" + "</font>" +
                                    "<BR>");
                            if (mod.debugMode)
                            {
                                gv.cc.addLogText("<font color='yellow'>" + saveChkRoll + " + " + pc.reflex + " >= " + DC + "</font>" +
                                            "<BR>");
                            }
                        }
                        else //failed check
                        {
                            gv.cc.addLogText("<font color='red'>" + pc.name + " is poisoned" + "</font>" + "<BR>");
                            Effect ef = mod.getEffectByTag("poisonedLight");
                            pc.AddEffectByObject(ef, 1);
                        }
                    }
                    else if (filename.Equals("onHitPcPoisonedMedium.cs"))
                    {
                        int saveChkRoll = gv.sf.RandInt(20);
                        int saveChk = saveChkRoll + pc.reflex;
                        int DC = 16;
                        if (saveChk >= DC) //passed save check
                        {
                            gv.cc.addLogText("<font color='yellow'>" + pc.name + " avoids being poisoned" + "</font>" +
                                    "<BR>");
                            if (mod.debugMode)
                            {
                                gv.cc.addLogText("<font color='yellow'>" + saveChkRoll + " + " + pc.reflex + " >= " + DC + "</font>" +
                                            "<BR>");
                            }
                        }
                        else //failed check
                        {
                            gv.cc.addLogText("<font color='red'>" + pc.name + " is poisoned" + "</font>" + "<BR>");
                            Effect ef = mod.getEffectByTag("poisonedMedium");
                            pc.AddEffectByObject(ef, 1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    gv.errorLog(ex.ToString());
                }
            }
        }
        public void doCreatureOnHitCastSpell(Creature crt, Player pc)
        {
            Spell sp = gv.mod.getSpellByTag(crt.onScoringHitCastSpellTag);
            if (sp == null) { return; }
            gv.cc.doSpellBasedOnScriptOrEffectTag(sp, crt, pc);
        }
        public bool checkEndEncounter()
        {
            int foundOneCrtr = 0;
            foreach (Creature crtr in mod.currentEncounter.encounterCreatureList)
            {
                if (crtr.hp > 0)
                {
                    foundOneCrtr = 1;
                }
            }
            if ((foundOneCrtr == 0) && (gv.screenType.Equals("combat")))
            {
                gv.touchEnabled = true;
                // give gold drop
                if (mod.currentEncounter.goldDrop > 0)
                {
                    gv.cc.addLogText("<font color='yellow'>The party finds " + mod.currentEncounter.goldDrop + " " + mod.goldLabelPlural + ".<BR></font>");
                }
                mod.partyGold += mod.currentEncounter.goldDrop;
                // give InventoryList
                if (mod.currentEncounter.encounterInventoryRefsList.Count > 0)
                {

                    string s = "<font color='fuchsia'>" + "The party has found:<BR>";
                    foreach (ItemRefs itRef in mod.currentEncounter.encounterInventoryRefsList)
                    {
                        mod.partyInventoryRefsList.Add(itRef.DeepCopy());
                        s += itRef.name + "<BR>";
                        //find this creatureRef in mod creature list

                    }
                    gv.cc.addLogText(s + "</font>" + "<BR>");
                }

                int giveEachXP = encounterXP / mod.playerList.Count;
                gv.cc.addLogText("fuchsia", "Each receives " + giveEachXP + " XP");
                foreach (Player givePcXp in mod.playerList)
                {
                    givePcXp.XP = givePcXp.XP + giveEachXP;
                }
                //btnSelect.Text = "SELECT";
                gv.screenType = "main";
                if (mod.playMusic)
                {
                    gv.stopCombatMusic();
                    gv.startMusic();
                    gv.startAmbient();
                }
                //do END ENCOUNTER IBScript
                gv.cc.doIBScriptBasedOnFilename(gv.mod.currentEncounter.OnEndCombatIBScript, gv.mod.currentEncounter.OnEndCombatIBScriptParms);
                if (gv.cc.calledEncounterFromProp)
                {
                    //gv.mod.isRecursiveDoTriggerCallMovingProp = true;
                    //gv.mod.isRecursiveCall = true;
                    gv.cc.doPropTriggers();
                    //gv.mod.isRecursiveCall = false;
                }
                else
                {
                    gv.cc.doTrigger();
                }
                return true;
            }

            int foundOnePc = 0;
            foreach (Player pc in mod.playerList)
            {
                if (pc.hp > 0)
                {
                    foundOnePc = 1;
                }
            }
            if (foundOnePc == 0)
            {
                gv.touchEnabled = true;
                gv.sf.MessageBox("Your party has been defeated!");
                if (mod.playMusic)
                {
                    gv.stopCombatMusic();
                    gv.startMusic();
                    gv.startAmbient();
                }
                gv.resetGame();
                gv.screenType = "title";
                return true;
            }
            return false;
        }

        //COMBAT SCREEN UPDATE
        public void Update(int elapsed)
        {
            combatUiLayout.Update(elapsed);

            #region PROP AMBIENT SPRITES
            foreach (Sprite spr in spriteList)
            {
                spr.Update(elapsed, gv);
            }
            //remove sprite if hit end of life
            for (int x = spriteList.Count - 1; x >= 0; x--)
            {
                if (spriteList[x].timeToLiveInMilliseconds <= 0)
                {
                    try
                    {
                        spriteList.RemoveAt(x);
                    }
                    catch (Exception ex)
                    {
                        gv.errorLog(ex.ToString());
                    }
                }
            }
            #endregion

            #region COMBAT ANIMATION SPRITES
            if (animationsOn)
            {
                attackAnimationTimeElapsed += elapsed;
                if (attackAnimationTimeElapsed >= attackAnimationLengthInMilliseconds)
                {
                    //time is up, reset attack animations to null
                    creatureToAnimate = null;
                    playerToAnimate = null;
                    foreach (AnimationSequence seq in animationSeqStack)
                    {
                        if(seq.AnimationSeq[0].turnFloatyTextOn)
                        {
                            floatyTextOn = true; //show any floaty text in the pool
                        }
                        foreach (Sprite spr in seq.AnimationSeq[0].SpriteGroup)
                        {
                            //just update the group at the top of the stack, first in first
                            spr.Update(elapsed, gv);
                        }
                    }
                    //remove sprites if hit end of life
                    for (int aniseq = animationSeqStack.Count - 1; aniseq >= 0; aniseq--)
                    {
                        for (int stkgrp = animationSeqStack[aniseq].AnimationSeq.Count - 1; stkgrp >= 0; stkgrp--)
                        {
                            for (int sprt = animationSeqStack[aniseq].AnimationSeq[stkgrp].SpriteGroup.Count - 1; sprt >= 0; sprt--)
                            {
                                if (animationSeqStack[aniseq].AnimationSeq[stkgrp].SpriteGroup[sprt].timeToLiveInMilliseconds <= 0)
                                {
                                    try
                                    {
                                        animationSeqStack[aniseq].AnimationSeq[stkgrp].SpriteGroup.RemoveAt(sprt);
                                       
                                    }
                                    catch (Exception ex)
                                    {
                                        gv.errorLog(ex.ToString());
                                    }
                                }
                            }
                            if (animationSeqStack[aniseq].AnimationSeq[stkgrp].SpriteGroup.Count == 0)
                            {
                                try
                                {
                                    animationSeqStack[aniseq].AnimationSeq.RemoveAt(stkgrp);
                                }
                                catch (Exception ex)
                                {
                                    gv.errorLog(ex.ToString());
                                }
                            }
                        }
                        if (animationSeqStack[aniseq].AnimationSeq.Count == 0)
                        {
                            try
                            {
                                animationSeqStack.RemoveAt(aniseq);
                            }
                            catch (Exception ex)
                            {
                                gv.errorLog(ex.ToString());
                            }
                        }
                    }
                    //if all animation sequences are done, end this turn
                    if (animationSeqStack.Count == 0)
                    {
                        animationsOn = false;                        
                        deathAnimationLocations.Clear();
                        
                        //remove any dead creatures                        
                        for (int x = mod.currentEncounter.encounterCreatureList.Count - 1; x >= 0; x--)
                        {
                            if (mod.currentEncounter.encounterCreatureList[x].hp <= 0)
                            {
                                try
                                {
                                    //do OnDeath IBScript
                                    gv.cc.doIBScriptBasedOnFilename(mod.currentEncounter.encounterCreatureList[x].onDeathIBScript, mod.currentEncounter.encounterCreatureList[x].onDeathIBScriptParms);
                                    mod.currentEncounter.encounterCreatureList.RemoveAt(x);
                                    mod.currentEncounter.encounterCreatureRefsList.RemoveAt(x);
                                }
                                catch (Exception ex)
                                {
                                    gv.errorLog(ex.ToString());
                                }
                            }
                        }
                        if (isPlayerTurn)
                        {
                            checkEndEncounter();
                            gv.touchEnabled = true;
                            animationState = AnimationState.None;
                            endPcTurn(true);
                        }
                        else
                        {
                            animationState = AnimationState.None;
                            endCreatureTurn();
                        }
  
                    }
                    
                }
                if ((gv.mod.useManualCombatCam) && (animationSeqStack.Count == 0))
                {
                    gv.touchEnabled = true;
                }
            }
            #endregion

            #region FLOATY TEXT
            if (floatyTextOn)
            {
                //move up 50pxl per second (50px/1000ms)*elapsed
                float multiplier = 100.0f / gv.mod.combatAnimationSpeed;
                int shiftUp = (int)(0.05f * elapsed * multiplier);
                foreach (FloatyText ft in gv.cc.floatyTextList)
                {
                    ft.location.Y -= shiftUp;
                    ft.timeToLive -= (int)(elapsed * multiplier);
                }
                //remove sprite if hit end of life
                for (int x = gv.cc.floatyTextList.Count - 1; x >= 0; x--)
                {
                    if (gv.cc.floatyTextList[x].timeToLive <= 0)
                    {
                        try
                        {
                            gv.cc.floatyTextList.RemoveAt(x);
                        }
                        catch (Exception ex)
                        {
                            gv.errorLog(ex.ToString());
                        }
                    }
                }
                if (gv.cc.floatyTextList.Count == 0)
                {
                    floatyTextOn = false;
                }                
            }
            #endregion
        }
        
        #region Combat Draw
        public void redrawCombat()
        {
            drawCombatMap();
            if (gv.mod.useCombatSmoothMovement == false)
            {
                drawCombatCreatures();
            }
            else
            {
                drawMovingCombatCreatures();
            }
            drawCombatPlayers();
            drawSprites();
            if (mod.currentEncounter.UseDayNightCycle)
            {
                drawOverlayTints();
            }
            if (!animationsOn)
            {
                drawTargetHighlight();
                drawLosTrail();
            }
            drawFloatyText();
            drawHPText();
            drawSPText();
            drawFloatyTextList();
            //started work on iniative bar for creatures and pc
            if (showIniBar)
            {
                for (int j = 0; j < combatUiLayout.panelList.Count; j++)
                {
                    if (combatUiLayout.panelList[j].tag.Equals("InitiativePanel"))
                    {
                        if ((combatUiLayout.panelList[j].currentLocX) > (combatUiLayout.panelList[j].hiddenLocX))
                        {
                            if (ListOfCreaturesDisplayedInCurrentBar.Count > 0)
                            {
                                drawInitiativeBar();
                            }
                        }
                    }
                }
            }
            drawUiLayout();
        }

        public void recalculateCreaturesShownInInitiativeBar()
        {
            numberOfCurrentlyShownBar = 1;

            ListOfCreaturesDisplayedInCurrentBar.Clear();
            ListOfSizesOfCreaturesInCurrentBar.Clear();

            ListOfCreaturesDisplayedInBar1.Clear();
            ListOfCreaturesDisplayedInBar2.Clear();
            ListOfCreaturesDisplayedInBar3.Clear();
            ListOfCreaturesDisplayedInBar4.Clear();
            ListOfCreaturesDisplayedInBar5.Clear();
            ListOfCreaturesDisplayedInBar6.Clear();

            ListOfSizesOfCreaturesInBar1.Clear();
            ListOfSizesOfCreaturesInBar2.Clear();
            ListOfSizesOfCreaturesInBar3.Clear();
            ListOfSizesOfCreaturesInBar4.Clear();
            ListOfSizesOfCreaturesInBar5.Clear();
            ListOfSizesOfCreaturesInBar6.Clear();

            NumberOfButtonsDisplayedInBar1 = 0;
            NumberOfButtonsDisplayedInBar2 = 0;
            NumberOfButtonsDisplayedInBar3 = 0;
            NumberOfButtonsDisplayedInBar4 = 0;
            NumberOfButtonsDisplayedInBar5 = 0;
            NumberOfButtonsDisplayedInBar6 = 0;

            NumberOfBackgroundTilesDisplayedInBar1 = 0;
            NumberOfBackgroundTilesDisplayedInBar2 = 0;
            NumberOfBackgroundTilesDisplayedInBar3 = 0;
            NumberOfBackgroundTilesDisplayedInBar4 = 0;
            NumberOfBackgroundTilesDisplayedInBar5 = 0;
            NumberOfBackgroundTilesDisplayedInBar6 = 0;

            moverOrdersOfAllLivingCreatures.Clear();
            moverOrdersOfAllFallenCreatures.Clear();
            moverOrdersOfLargeLivingCreatures.Clear();
            moverOrdersOfLargeFallenCreatures.Clear();
            moverOrdersOfNormalLivingCreatures.Clear();
            moverOrdersOfNormalFallenCreatures.Clear();

            foreach (MoveOrder m in moveOrderList)
            {
                if (m.PcOrCreature is Player)
                {
                    Player crt = (Player)m.PcOrCreature;
                    if (crt.hp <= 0)
                    {
                        if (crt.token.PixelSize.Width > 100)
                        {
                            moverOrdersOfLargeFallenCreatures.Add(crt.moveOrder);
                        }
                        else
                        {
                            moverOrdersOfNormalFallenCreatures.Add(crt.moveOrder);
                        }
                    }
                    else
                    {
                        moverOrdersOfAllLivingCreatures.Add(crt.moveOrder);
                        if (crt.token.PixelSize.Width > 100)
                        {
                            moverOrdersOfLargeLivingCreatures.Add(crt.moveOrder);
                        }
                        else
                        {
                            moverOrdersOfNormalLivingCreatures.Add(crt.moveOrder);
                        }
                    }
                }
                else
                {
                    Creature crt = (Creature)m.PcOrCreature;
                    if (crt.hp <= 0)
                    {
                        if (crt.token.PixelSize.Width > 100)
                        {
                            moverOrdersOfLargeFallenCreatures.Add(crt.moveOrder);
                        }
                        else
                        {
                            moverOrdersOfNormalFallenCreatures.Add(crt.moveOrder);
                        }
                    }
                    else
                    {
                        moverOrdersOfAllLivingCreatures.Add(crt.moveOrder);
                        if (crt.token.PixelSize.Width > 100)
                        {
                            moverOrdersOfLargeLivingCreatures.Add(crt.moveOrder);
                        }
                        else
                        {
                            moverOrdersOfNormalLivingCreatures.Add(crt.moveOrder);
                        }
                    }
                }
            }

            //calculate buttons/i needed
            int buttonsNeededOverall = 0;

            buttonsNeededOverall = (moverOrdersOfLargeLivingCreatures.Count) * 2;
            buttonsNeededOverall += moverOrdersOfNormalLivingCreatures.Count;

            //determine number of inibars needed
            int numberOfIniBarsNeeded = 1;

            //not certain of rounding rules, too lazy to look up ;-)
            for (int i = 1; i <= buttonsNeededOverall; i++)
            {
                if (i == 35)
                {
                    numberOfIniBarsNeeded++;
                }

                if (i == 69)
                {
                    numberOfIniBarsNeeded++;
                }

                if (i == 103)
                {
                    numberOfIniBarsNeeded++;
                }

                if (i == 137)
                {
                    numberOfIniBarsNeeded++;
                }

                if (i == 171)
                {
                    numberOfIniBarsNeeded++;
                }  
            }

            int indexIAdderForDrawnLargeCreatures = 0;
            for (int barNumber = 1; barNumber <= numberOfIniBarsNeeded; barNumber++)
            {
                int nextBarMoveOrderAdder = (barNumber - 1) * 34;

                for (int i = 0 + nextBarMoveOrderAdder; i < 34 + nextBarMoveOrderAdder; i++)
                {
                    if (barNumber == 1)
                    {
                        if ((i - indexIAdderForDrawnLargeCreatures) <= moverOrdersOfAllLivingCreatures.Count - 1)
                        {
                            ListOfCreaturesDisplayedInBar1.Add(moverOrdersOfAllLivingCreatures[i - indexIAdderForDrawnLargeCreatures]);
                            bool foundLargeCreature = false;
                            foreach (int moveOrder in moverOrdersOfLargeLivingCreatures)
                            {
                                if (moveOrder == moverOrdersOfAllLivingCreatures[i - indexIAdderForDrawnLargeCreatures])
                                {
                                    foundLargeCreature = true;
                                    ListOfSizesOfCreaturesInBar1.Add(2);
                                    indexIAdderForDrawnLargeCreatures++;
                                    i++;
                                    break;
                                }
                            }

                            if (!foundLargeCreature)
                            {
                                ListOfSizesOfCreaturesInBar1.Add(1);
                            }
                        }
                    }

                    if (barNumber == 2)
                    {
                        if ((i - indexIAdderForDrawnLargeCreatures) <= moverOrdersOfAllLivingCreatures.Count - 1)
                        {
                            ListOfCreaturesDisplayedInBar2.Add(moverOrdersOfAllLivingCreatures[i - indexIAdderForDrawnLargeCreatures]);
                            bool foundLargeCreature = false;
                            foreach (int moveOrder in moverOrdersOfLargeLivingCreatures)
                            {
                                if (moveOrder == moverOrdersOfAllLivingCreatures[i - indexIAdderForDrawnLargeCreatures])
                                {
                                    foundLargeCreature = true;
                                    ListOfSizesOfCreaturesInBar2.Add(2);
                                    indexIAdderForDrawnLargeCreatures++;
                                    i++;
                                    break;
                                }
                            }

                            if (!foundLargeCreature)
                            {
                                ListOfSizesOfCreaturesInBar2.Add(1);
                            }
                        }
                    }

                    if (barNumber == 3)
                    {
                        if ((i - indexIAdderForDrawnLargeCreatures) <= moverOrdersOfAllLivingCreatures.Count - 1)
                        {
                            ListOfCreaturesDisplayedInBar3.Add(moverOrdersOfAllLivingCreatures[i - indexIAdderForDrawnLargeCreatures]);
                            bool foundLargeCreature = false;
                            foreach (int moveOrder in moverOrdersOfLargeLivingCreatures)
                            {
                                if (moveOrder == moverOrdersOfAllLivingCreatures[i - indexIAdderForDrawnLargeCreatures])
                                {
                                    foundLargeCreature = true;
                                    ListOfSizesOfCreaturesInBar3.Add(2);
                                    indexIAdderForDrawnLargeCreatures++;
                                    i++;
                                    break;
                                }
                            }

                            if (!foundLargeCreature)
                            {
                                ListOfSizesOfCreaturesInBar3.Add(1);
                            }
                        }
                    }

                    if (barNumber == 4)
                    {
                        if ((i - indexIAdderForDrawnLargeCreatures) <= moverOrdersOfAllLivingCreatures.Count - 1)
                        {
                            ListOfCreaturesDisplayedInBar4.Add(moverOrdersOfAllLivingCreatures[i - indexIAdderForDrawnLargeCreatures]);
                            bool foundLargeCreature = false;
                            foreach (int moveOrder in moverOrdersOfLargeLivingCreatures)
                            {
                                if (moveOrder == moverOrdersOfAllLivingCreatures[i - indexIAdderForDrawnLargeCreatures])
                                {
                                    foundLargeCreature = true;
                                    ListOfSizesOfCreaturesInBar4.Add(2);
                                    indexIAdderForDrawnLargeCreatures++;
                                    i++;
                                    break;
                                }
                            }

                            if (!foundLargeCreature)
                            {
                                ListOfSizesOfCreaturesInBar4.Add(1);
                            }
                        }
                    }

                    if (barNumber == 5)
                    {
                        if ((i - indexIAdderForDrawnLargeCreatures) <= moverOrdersOfAllLivingCreatures.Count - 1)
                        {
                            ListOfCreaturesDisplayedInBar5.Add(moverOrdersOfAllLivingCreatures[i - indexIAdderForDrawnLargeCreatures]);
                            bool foundLargeCreature = false;
                            foreach (int moveOrder in moverOrdersOfLargeLivingCreatures)
                            {
                                if (moveOrder == moverOrdersOfAllLivingCreatures[i - indexIAdderForDrawnLargeCreatures])
                                {
                                    foundLargeCreature = true;
                                    ListOfSizesOfCreaturesInBar5.Add(2);
                                    indexIAdderForDrawnLargeCreatures++;
                                    i++;
                                    break;
                                }
                            }

                            if (!foundLargeCreature)
                            {
                                ListOfSizesOfCreaturesInBar5.Add(1);
                            }
                        }
                    }

                    if (barNumber == 6)
                    {
                        if ((i - indexIAdderForDrawnLargeCreatures) <= moverOrdersOfAllLivingCreatures.Count - 1)
                        {
                            ListOfCreaturesDisplayedInBar6.Add(moverOrdersOfAllLivingCreatures[i - indexIAdderForDrawnLargeCreatures]);
                            bool foundLargeCreature = false;
                            foreach (int moveOrder in moverOrdersOfLargeLivingCreatures)
                            {
                                if (moveOrder == moverOrdersOfAllLivingCreatures[i - indexIAdderForDrawnLargeCreatures])
                                {
                                    foundLargeCreature = true;
                                    ListOfSizesOfCreaturesInBar6.Add(2);
                                    indexIAdderForDrawnLargeCreatures++;
                                    i++;
                                    break;
                                }
                            }

                            if (!foundLargeCreature)
                            {
                                ListOfSizesOfCreaturesInBar6.Add(1);
                            }

                        }

                    }
                }
            }

            //determine current bar
            bool foundCurrentBar = false;

            foreach (int moveOrder in ListOfCreaturesDisplayedInBar1)
            {
                if (moveOrder == (currentMoveOrderIndex))
                {
                    ListOfCreaturesDisplayedInCurrentBar = ListOfCreaturesDisplayedInBar1;
                    ListOfSizesOfCreaturesInCurrentBar= ListOfSizesOfCreaturesInBar1;
                    foundCurrentBar = true;
                    gv.mod.creatureCounterSubstractor = 0;
                }
            }

            if (!foundCurrentBar)
            {
                foreach (int moveOrder in ListOfCreaturesDisplayedInBar2)
                {
                    if (moveOrder == (currentMoveOrderIndex))
                    {
                        ListOfCreaturesDisplayedInCurrentBar = ListOfCreaturesDisplayedInBar2;
                        ListOfSizesOfCreaturesInCurrentBar = ListOfSizesOfCreaturesInBar2;
                        foundCurrentBar = true;
                        gv.mod.creatureCounterSubstractor = -34;
                    }
                }
            }

            if (!foundCurrentBar)
            {
                foreach (int moveOrder in ListOfCreaturesDisplayedInBar3)
                {
                    if (moveOrder == (currentMoveOrderIndex))
                    {
                        ListOfCreaturesDisplayedInCurrentBar = ListOfCreaturesDisplayedInBar3;
                        ListOfSizesOfCreaturesInCurrentBar = ListOfSizesOfCreaturesInBar3;
                        foundCurrentBar = true;
                        gv.mod.creatureCounterSubstractor = -68;
                    }
                }
            }

            if (!foundCurrentBar)
            {
                foreach (int moveOrder in ListOfCreaturesDisplayedInBar4)
                {
                    if (moveOrder == (currentMoveOrderIndex))
                    {
                        ListOfCreaturesDisplayedInCurrentBar = ListOfCreaturesDisplayedInBar4;
                        ListOfSizesOfCreaturesInCurrentBar = ListOfSizesOfCreaturesInBar4;
                        foundCurrentBar = true;
                        gv.mod.creatureCounterSubstractor = -102;
                    }
                }
            }

            if (!foundCurrentBar)
            {
                foreach (int moveOrder in ListOfCreaturesDisplayedInBar5)
                {
                    if (moveOrder == (currentMoveOrderIndex))
                    {
                        ListOfCreaturesDisplayedInCurrentBar = ListOfCreaturesDisplayedInBar5;
                        ListOfSizesOfCreaturesInCurrentBar = ListOfSizesOfCreaturesInBar5;
                        foundCurrentBar = true;
                        gv.mod.creatureCounterSubstractor = -136;
                    }
                }
            }

            if (!foundCurrentBar)
            {
                foreach (int moveOrder in ListOfCreaturesDisplayedInBar6)
                {
                    if (moveOrder == (currentMoveOrderIndex - 1))
                    {
                        ListOfCreaturesDisplayedInCurrentBar = ListOfCreaturesDisplayedInBar6;
                        ListOfSizesOfCreaturesInCurrentBar = ListOfSizesOfCreaturesInBar6;
                        foundCurrentBar = true;
                        gv.mod.creatureCounterSubstractor = -170;
                    }
                }
            }

            //actiavte the right number of buttons
            int numberOfIniButtonsToActivate = 0;
            foreach (int creatureSize in ListOfSizesOfCreaturesInCurrentBar)
            {
                numberOfIniButtonsToActivate += creatureSize;
            }

            for (int i = 0; i < numberOfIniButtonsToActivate; i++)
            {
                for (int j = 0; j < combatUiLayout.panelList.Count; j++)
                {
                    if (combatUiLayout.panelList[j].tag.Equals("InitiativePanel"))
                    {
                        if (!combatUiLayout.panelList[j].hiding)
                        {
                            combatUiLayout.panelList[j].buttonList[i].show = true;
                        }
                    }
                }
            }

            for (int i = 35; i > numberOfIniButtonsToActivate; i--)
            {
                for (int j = 0; j < combatUiLayout.panelList.Count; j++)
                {
                    if (combatUiLayout.panelList[j].tag.Equals("InitiativePanel"))
                    {
                        if (!combatUiLayout.panelList[j].hiding)
                        {
                            combatUiLayout.panelList[j].buttonList[i].show = false;
                        }
                    }
                }
            }
        }

        public void drawInitiativeBar()
        {

            //draw background as tiles needed
            int numberOfBackgroundTilesToDraw = 0;
            int creatureSpacesUsed = 0;
            foreach (int creatureSize in ListOfSizesOfCreaturesInCurrentBar)
            {
                numberOfBackgroundTilesToDraw += creatureSize;
            }

            if (ListOfSizesOfCreaturesInCurrentBar[ListOfSizesOfCreaturesInCurrentBar.Count - 1] == 2)
            {
                if (numberOfBackgroundTilesToDraw % 2 != 0)
                {
                    numberOfBackgroundTilesToDraw += 1;
                }
            }

            if (numberOfBackgroundTilesToDraw % 2 != 0)
            {
                numberOfBackgroundTilesToDraw += 1;
            }

            numberOfBackgroundTilesToDraw = numberOfBackgroundTilesToDraw / 2;

            for (int i = 0; i < numberOfBackgroundTilesToDraw; i++)
            {
                int startBarX = (0 * gv.squareSize) + gv.oXshift + mapStartLocXinPixels + 2 * gv.pS;
                int startBarY = 0 * gv.squareSize + 2 * gv.pS;
                int targetSizeX = gv.squareSize;
                int targetSizeY = gv.squareSize;
                IbRect src = new IbRect(0, 0, 100, 100);
                IbRect dst = new IbRect(startBarX + i * gv.squareSize - (int)(targetSizeX * 0.1), startBarY - (int)(targetSizeY * 0.1), (int)(targetSizeX * 1.2f), (int)(targetSizeY * 1.2f));
                gv.DrawBitmap(gv.cc.offScreen, src, dst, false);
            }

            //draw creature in current bar
            foreach (MoveOrder m in moveOrderList)
            {
                int moveOrderNumberOfCheckedCreatureFromAllCreatures = 0;
                bool isCreature = true;
                Player ply = new Player();
                Creature crt = new Creature();

                if (m.PcOrCreature is Player)
                {
                    isCreature = false;
                    ply = (Player)m.PcOrCreature;
                    moveOrderNumberOfCheckedCreatureFromAllCreatures = ply.moveOrder;
                }
                if (m.PcOrCreature is Creature)
                {
                    isCreature = true;
                    crt = (Creature)m.PcOrCreature;
                    moveOrderNumberOfCheckedCreatureFromAllCreatures = crt.moveOrder;
                }

                foreach (int m2 in ListOfCreaturesDisplayedInCurrentBar)
                {
                    if (m2 == moveOrderNumberOfCheckedCreatureFromAllCreatures)
                    {
                        if (isCreature)
                        {
                            IbRect src = new IbRect(0, 0, crt.token.PixelSize.Width, crt.token.PixelSize.Width);
                            int startBarX = (0 * gv.squareSize) + gv.oXshift + mapStartLocXinPixels + 2 * gv.pS;
                            int startBarY = 0 * gv.squareSize + 2 * gv.pS;
                            int targetSizeX = gv.squareSize / 2;
                            int targetSizeY = gv.squareSize / 2;
                            int marchingLineHeight = gv.squareSize / 2;
                            if (crt.token.PixelSize.Width > 100)
                            {
                                targetSizeX = gv.squareSize;
                                targetSizeY = gv.squareSize;
                                marchingLineHeight = 0;
                            }
                            IbRect dst = new IbRect(startBarX + creatureSpacesUsed * gv.squareSize / 2, startBarY + marchingLineHeight, targetSizeX, targetSizeY);
                            if (crt.moveOrder + 1 == currentMoveOrderIndex)
                            {
                                gv.DrawBitmap(gv.cc.turn_marker, src, dst, false);
                            }

                                gv.DrawBitmap(crt.token, src, dst, false);
                                int mo = crt.moveOrder + 1;
                                if (crt.token.PixelSize.Width <= 100)
                                {
                                    creatureSpacesUsed++;
                                    drawMiniText(dst.Left, dst.Top + 1 * gv.pS, mo.ToString(), Color.White);
                                    drawMiniText(dst.Left + gv.pS, dst.Top - 5 * gv.pS, crt.hp.ToString(), Color.Red);
                                }
                                else
                                {
                                    creatureSpacesUsed++;
                                    creatureSpacesUsed++;
                                    drawMiniText(dst.Left, dst.Top + gv.squareSize / 2 + 1 * gv.pS, mo.ToString(), Color.White);
                                    drawMiniText(dst.Left + 3 * gv.pS, dst.Top - 3 * gv.pS, crt.hp.ToString(), Color.Red);
                                }
                        }
                        else
                        {
                            IbRect src = new IbRect(0, 0, ply.token.PixelSize.Width, ply.token.PixelSize.Width);
                            int startBarX = (0 * gv.squareSize) + gv.oXshift + mapStartLocXinPixels + 2 * gv.pS;
                            int startBarY = 0 * gv.squareSize + 2 * gv.pS;
                            int targetSizeX = gv.squareSize / 2;
                            int targetSizeY = gv.squareSize / 2;
                            int marchingLineHeight = gv.squareSize / 2;
                            if (ply.token.PixelSize.Width > 100)
                            {
                                targetSizeX = gv.squareSize;
                                targetSizeY = gv.squareSize;
                                marchingLineHeight = 0;
                            }
                            IbRect dst = new IbRect(startBarX + creatureSpacesUsed * gv.squareSize / 2, startBarY + marchingLineHeight, targetSizeX, targetSizeY);
                            if (ply.moveOrder + 1 == currentMoveOrderIndex)
                            {
                                gv.DrawBitmap(gv.cc.turn_marker, src, dst, false);
                            }

                            gv.DrawBitmap(ply.token, src, dst, false);
                            int mo = ply.moveOrder + 1;
                            if (ply.token.PixelSize.Width <= 100)
                            {
                                creatureSpacesUsed++;
                                drawMiniText(dst.Left, dst.Top + 1 * gv.pS, mo.ToString(), Color.White);
                                drawMiniText(dst.Left + gv.pS, dst.Top - 5 * gv.pS, ply.hp.ToString(), Color.Lime);
                            }
                            else
                            {
                                creatureSpacesUsed++;
                                creatureSpacesUsed++;
                                drawMiniText(dst.Left, dst.Top + gv.squareSize / 2 + 1 * gv.pS, mo.ToString(), Color.White);
                                drawMiniText(dst.Left + 3 * gv.pS, dst.Top - 3 * gv.pS, ply.hp.ToString(), Color.Lime);
                            }
                        }

                    }
                }
            }
                   /*
                //draw turn marker
                //if (crt.moveOrder + 1 == currentMoveOrderIndex)
                //{
                //gv.DrawBitmap(gv.cc.turn_marker, src, dst, false);
                //}


                //XXXXXXXXXXXXXXXXXXXXXXXXXX

                if ((gv.mod.creatureCounterSubstractor / 2f == -16) && (!gv.mod.enteredFirstTime))
            {
                gv.mod.enteredFirstTime = true;
                gv.mod.moveOrderOfCreatureThatIsBeforeBandChange = currentMoveOrderIndex;
            }

            if ((gv.mod.creatureCounterSubstractor / 2f == -32) && (!gv.mod.enteredFirstTime))
            {
                gv.mod.enteredFirstTime = true;
                gv.mod.moveOrderOfCreatureThatIsBeforeBandChange = currentMoveOrderIndex;
            }

            if ((gv.mod.creatureCounterSubstractor / 2f == -48) && (!gv.mod.enteredFirstTime))
            {
                gv.mod.enteredFirstTime = true;
                gv.mod.moveOrderOfCreatureThatIsBeforeBandChange = currentMoveOrderIndex;
            }

            if ((gv.mod.creatureCounterSubstractor / 2f == -64) && (!gv.mod.enteredFirstTime))
            {
                gv.mod.enteredFirstTime = true;
                gv.mod.moveOrderOfCreatureThatIsBeforeBandChange = currentMoveOrderIndex;
            }

            if ((gv.mod.creatureCounterSubstractor / 2f == -80) && (!gv.mod.enteredFirstTime))
            {
                gv.mod.enteredFirstTime = true;
                gv.mod.moveOrderOfCreatureThatIsBeforeBandChange = currentMoveOrderIndex;
            }

            if ((gv.mod.creatureCounterSubstractor / 2f == -96) && (!gv.mod.enteredFirstTime))
            {
                gv.mod.enteredFirstTime = true;
                gv.mod.moveOrderOfCreatureThatIsBeforeBandChange = currentMoveOrderIndex;
            }
            //if (currentMoveOrderIndex > gv.mod.moveOrderOfCreatureThatIsBeforeBandChange)
            //{
            float numberOfBackgroundTiles = 0;
            if ((currentMoveOrderIndex > gv.mod.moveOrderOfCreatureThatIsBeforeBandChange) || (currentMoveOrderIndex == 0))
            {
                numberOfBackgroundTiles = (gv.mod.creatureCounterSubstractor / 2f);
                gv.mod.moveOrderOfCreatureThatIsBeforeBandChange = 0;
            }
             
            //}
            foreach (MoveOrder m in moveOrderList)
            {
                if (m.PcOrCreature is Player)
                {
                    Player crt = (Player)m.PcOrCreature;
                    if (crt.hp <= 0)
                    {
                        continue;
                    }
                    if (crt.token.PixelSize.Width > 100)
                    {
                        numberOfBackgroundTiles++;
                    }
                    else
                    {
                        numberOfBackgroundTiles += 0.5f;
                    }
                }

                if (m.PcOrCreature is Creature)
                {
                    Creature crt = (Creature)m.PcOrCreature;
                    if (crt.hp <= 0)
                    {
                        continue;
                    }
                    if (crt.token.PixelSize.Width > 100)
                    {
                        numberOfBackgroundTiles++;
                    }
                    else
                    {
                        numberOfBackgroundTiles += 0.5f;
                    }
                }

            }

            if (numberOfBackgroundTiles > 16)
            {
                numberOfBackgroundTiles = 16;
            }

            int numberOfIniButtonsToActivate = (int)(numberOfBackgroundTiles * 2);

            for (int i = 0; i < numberOfIniButtonsToActivate; i++)
            {
                for (int j = 0; j < combatUiLayout.panelList.Count; j++)
                {
                    if (combatUiLayout.panelList[j].tag.Equals("InitiativePanel"))
                    {
                        if (!combatUiLayout.panelList[j].hiding)
                        {
                            combatUiLayout.panelList[j].buttonList[i].show = true;
                        }
                    }
               }
            }

            for (int i = 35; i > numberOfIniButtonsToActivate; i--)
            {
                for (int j = 0; j < combatUiLayout.panelList.Count; j++)
                {
                    if (combatUiLayout.panelList[j].tag.Equals("InitiativePanel"))
                    {
                        if (!combatUiLayout.panelList[j].hiding)
                        {
                            combatUiLayout.panelList[j].buttonList[i].show = false;
                        }
                    }
                }
            }


            for (int i = 0; i < numberOfBackgroundTiles; i++)
                {
                    int startBarX = (0 * gv.squareSize) + gv.oXshift + mapStartLocXinPixels + 2*gv.pS;
                    int startBarY = 0 * gv.squareSize + 2*gv.pS;
                    int targetSizeX = gv.squareSize;
                    int targetSizeY = gv.squareSize;
                    IbRect src = new IbRect(0, 0, 100, 100);
                    IbRect dst = new IbRect(startBarX + i * gv.squareSize - (int)(targetSizeX * 0.1), startBarY - (int)(targetSizeY * 0.1), (int)(targetSizeX * 1.2f), (int)(targetSizeY * 1.2f));
                    gv.DrawBitmap(gv.cc.offScreen, src, dst, false);
                }
            
            int creatureCounter = 0;
            int creatureCounter2 = 0;
            int adderForTheFallen = 0;
            int largeCreaturesInBand = 0;
            bool enteredBand2ForFirstTime = true;
            bool enteredBand3ForFirstTime = true;
            bool enteredBand4ForFirstTime = true;
            bool enteredBand5ForFirstTime = true;
            bool enteredBand6ForFirstTime = true;
            int largeCreaturesInBandBeforeThisOne = 0;

            foreach (MoveOrder m in moveOrderList)
            {
                if (m.PcOrCreature is Player)
                {
                    Player crt = (Player)m.PcOrCreature;

                    if (crt.hp <= 0)
                    {
                        adderForTheFallen++;
                        if (crt.token.PixelSize.Width > 100)
                        {
                            adderForTheFallen++;
                        }
                    }
                }

                if (m.PcOrCreature is Creature)
                {
                    Creature crt = (Creature)m.PcOrCreature;

                    if (crt.hp <= 0)
                    {
                        adderForTheFallen++;
                        if (crt.token.PixelSize.Width > 100)
                        {
                            adderForTheFallen++;
                        }
                    }
                }

            }


                foreach (MoveOrder m in moveOrderList)
            {
                if (creatureCounter2 < 32)
                {
                    if ((currentMoveOrderIndex - adderForTheFallen) < 32)
                    {
                        gv.mod.creatureCounterSubstractor = 0;
                    }
                }
                else if ((creatureCounter2 >= 32) && (creatureCounter2 <= 64))
                {
                    if ((currentMoveOrderIndex - adderForTheFallen) <= 64)
                    {
                        if (enteredBand2ForFirstTime)
                        {
                            enteredBand2ForFirstTime = false;
                            largeCreaturesInBandBeforeThisOne = largeCreaturesInBand;
                            largeCreaturesInBand = 0;
                        }
                        gv.mod.creatureCounterSubstractor = -32;
                    }
                }
                else if ((creatureCounter2 >= 64) && (creatureCounter2 <= 96))
                {
                    if ((currentMoveOrderIndex - adderForTheFallen) <= 96)
                    {
                        if (enteredBand3ForFirstTime)
                        {
                            enteredBand3ForFirstTime = false;
                            largeCreaturesInBandBeforeThisOne = largeCreaturesInBand;
                            largeCreaturesInBand = 0;
                        }
                        gv.mod.creatureCounterSubstractor = -64;
                    }
                }
                else if ((creatureCounter2 >= 96) && (creatureCounter2 <= 128))
                {
                    if ((currentMoveOrderIndex - adderForTheFallen) <= 128)
                    {
                        if (enteredBand4ForFirstTime)
                        {
                            enteredBand4ForFirstTime = false;
                            largeCreaturesInBandBeforeThisOne = largeCreaturesInBand;
                            largeCreaturesInBand = 0;
                        }
                        gv.mod.creatureCounterSubstractor = -96;
                    }
                }
                else if ((creatureCounter2 >= 128) && (creatureCounter2 <= 160))
                {
                    if ((currentMoveOrderIndex - adderForTheFallen) <= 160)
                    {
                        if (enteredBand5ForFirstTime)
                        {
                            enteredBand5ForFirstTime = false;
                            largeCreaturesInBandBeforeThisOne = largeCreaturesInBand;
                            largeCreaturesInBand = 0;
                        }
                        gv.mod.creatureCounterSubstractor = -128;
                    }
                }
                else if ((creatureCounter2 >= 160) && (creatureCounter2 <= 192))
                {
                    if ((currentMoveOrderIndex - adderForTheFallen) <= 192)
                    {
                        if (enteredBand6ForFirstTime)
                        {
                            enteredBand6ForFirstTime = false;
                            largeCreaturesInBandBeforeThisOne = largeCreaturesInBand;
                            largeCreaturesInBand = 0;
                        }
                        gv.mod.creatureCounterSubstractor = -160;
                    }
                }

                if (m.PcOrCreature is Player)
                {
                    Player crt = (Player)m.PcOrCreature;

                    if (crt.hp <= 0)
                    {
                        continue;
                    }
                    IbRect src = new IbRect(0, 0, crt.token.PixelSize.Width, crt.token.PixelSize.Width);
                    int startBarX = (0 * gv.squareSize) + gv.oXshift + mapStartLocXinPixels + 2*gv.pS;
                    int startBarY = 0 * gv.squareSize + 2*gv.pS;
                    int targetSizeX = gv.squareSize / 2;
                    int targetSizeY = gv.squareSize / 2;
                    int marchingLineHeight = gv.squareSize / 2;
                    if (crt.token.PixelSize.Width > 100)
                    {
                        targetSizeX = gv.squareSize;
                        targetSizeY = gv.squareSize;
                        marchingLineHeight = 0;
                    }
                    IbRect dst = new IbRect(startBarX + (creatureCounter + gv.mod.creatureCounterSubstractor) * gv.squareSize / 2, startBarY + marchingLineHeight, targetSizeX, targetSizeY);
                    if (crt.moveOrder+1 == currentMoveOrderIndex)
                    {
                        gv.DrawBitmap(gv.cc.turn_marker, src, dst, false);
                    }

                    if ((crt.moveOrder + 1 + largeCreaturesInBandBeforeThisOne + adderForTheFallen > -gv.mod.creatureCounterSubstractor) && (crt.moveOrder + largeCreaturesInBand - adderForTheFallen < (-gv.mod.creatureCounterSubstractor + 32)))
                    {
                        gv.DrawBitmap(crt.token, src, dst, false);
                        int mo = crt.moveOrder + 1;
                        if (crt.token.PixelSize.Width <= 100)
                        {
                            drawMiniText(dst.Left, dst.Top + 1 * gv.pS, mo.ToString(), Color.White);
                            drawMiniText(dst.Left + gv.pS, dst.Top - 5 * gv.pS, crt.hp.ToString(), Color.Lime);
                        }
                        else
                        {
                            drawMiniText(dst.Left, dst.Top + gv.squareSize / 2 + 1 * gv.pS, mo.ToString(), Color.White);
                            drawMiniText(dst.Left + 3 * gv.pS, dst.Top - 3 * gv.pS, crt.hp.ToString(), Color.Lime);
                        }
                    }

                    creatureCounter++;
                    if (crt.moveOrder + 1 <= currentMoveOrderIndex)
                    {
                        creatureCounter2++;
                    }
                        if (crt.token.PixelSize.Width > 100)
                    {
                        creatureCounter++;
                        if (crt.moveOrder + 1 <= currentMoveOrderIndex)
                        {
                            largeCreaturesInBand++;
                            creatureCounter2++;
                        }
                    }
                }

                else
                {
                    Creature crt = (Creature)m.PcOrCreature;
                    if (crt.hp <= 0)
                    {
                        continue;
                    }
                    IbRect src = new IbRect(0, 0, crt.token.PixelSize.Width, crt.token.PixelSize.Width);
                    int startBarX = (0 * gv.squareSize) + gv.oXshift + mapStartLocXinPixels + 2*gv.pS;
                    int startBarY = 0 * gv.squareSize + 2*gv.pS;
                    int targetSizeX = gv.squareSize / 2;
                    int targetSizeY = gv.squareSize / 2;
                    int marchingLineHeight = gv.squareSize / 2; ;
                    if (crt.token.PixelSize.Width > 100)
                    {
                        targetSizeX = gv.squareSize;
                        targetSizeY = gv.squareSize;
                        marchingLineHeight = 0;
                    }
                    IbRect dst = new IbRect(startBarX + (creatureCounter + gv.mod.creatureCounterSubstractor) * gv.squareSize / 2, startBarY + marchingLineHeight, targetSizeX, targetSizeY);
                    if (crt.moveOrder+1 == currentMoveOrderIndex)
                    {
                        gv.DrawBitmap(gv.cc.turn_marker, src, dst, false);
                    }

                    if ((crt.moveOrder + 1 + largeCreaturesInBandBeforeThisOne + adderForTheFallen > -gv.mod.creatureCounterSubstractor) && (crt.moveOrder + largeCreaturesInBand - adderForTheFallen < (-gv.mod.creatureCounterSubstractor + 32)))
                    {
                        gv.DrawBitmap(crt.token, src, dst, false);
                        int mo = crt.moveOrder + 1;
                        if (crt.token.PixelSize.Width <= 100)
                        {
                            drawMiniText(dst.Left, dst.Top + 1 * gv.pS, mo.ToString(), Color.White);
                            drawMiniText(dst.Left + gv.pS, dst.Top - 5 * gv.pS, crt.hp.ToString(), Color.Red);
                        }
                        else
                        {
                            drawMiniText(dst.Left, dst.Top + gv.squareSize / 2 + 1 * gv.pS, mo.ToString(), Color.White);
                            drawMiniText(dst.Left + 3 * gv.pS, dst.Top - 3 * gv.pS, crt.hp.ToString(), Color.Red);
                        }
                    }
                    creatureCounter++;
                    if (crt.moveOrder + 1 <= currentMoveOrderIndex)
                    {
                        creatureCounter2++;
                    }
                    if (crt.token.PixelSize.Width > 100)
                    {
                        largeCreaturesInBand++;
                        creatureCounter++;
                        if (crt.moveOrder + 1 <= currentMoveOrderIndex)
                        {
                            creatureCounter2++;
                        }
                    }
                }  
            }
           */
        }
        public void drawUiLayout()
        {
            //SET PORTRAITS
            foreach (IB2Panel pnl in combatUiLayout.panelList)
            {
                if (pnl.tag.Equals("portraitPanel"))
                {
                    foreach (IB2Portrait ptr in pnl.portraitList)
                    {
                        ptr.show = false;
                    }
                    int index = 0;
                    foreach (Player pc1 in mod.playerList)
                    {
                        pnl.portraitList[index].show = true;
                        pnl.portraitList[index].ImgFilename = pc1.portraitFilename;
                        pnl.portraitList[index].TextHP = pc1.hp + "/" + pc1.hpMax;
                        pnl.portraitList[index].TextSP = pc1.sp + "/" + pc1.spMax;
                        if (gv.mod.selectedPartyLeader == index)
                        {
                            pnl.portraitList[index].glowOn = true;
                        }
                        else
                        {
                            pnl.portraitList[index].glowOn = false;
                        }
                        index++;
                    }
                    break;
                }
            }

            //SET MOVES LEFT TEXT
            Player pc = mod.playerList[currentPlayerIndex];
            float movesLeft = pc.moveDistance - currentMoves;
            if (movesLeft < 0) { movesLeft = 0; }
            IB2Button btn = combatUiLayout.GetButtonByTag("btnMoveCounter");
            if (btn != null)
            {
                btn.Text = movesLeft.ToString();
            }

            //SET KILL BUTTON
            if (mod.debugMode)
            {
                IB2ToggleButton tgl = combatUiLayout.GetToggleByTag("tglKill");
                if (tgl != null)
                {
                    tgl.show = true;
                }
            }
            else
            {
                IB2ToggleButton tgl = combatUiLayout.GetToggleByTag("tglKill");
                if (tgl != null)
                {
                    tgl.show = false;
                }
            }

            //SET BUTTON STATES
            //select button
            if ((currentCombatMode.Equals("attack")) || (currentCombatMode.Equals("cast")))
            {
                btn = combatUiLayout.GetButtonByTag("btnSelect");
                if (btn != null)
                {
                    btn.Text = "TARGET";
                }
            }
            else
            {
                btn = combatUiLayout.GetButtonByTag("btnSelect");
                if (btn != null)
                {
                    btn.Text = "SELECT";
                }
            }
            //move button
            if (canMove)
            {
                if (currentCombatMode.Equals("move"))
                {
                    btn = combatUiLayout.GetButtonByTag("btnMove");
                    if (btn != null)
                    {
                        btn.btnState = buttonState.On;
                    }
                }
                else
                {
                    btn = combatUiLayout.GetButtonByTag("btnMove");
                    if (btn != null)
                    {
                        btn.btnState = buttonState.Normal;
                    }
                }
            }
            else
            {
                btn = combatUiLayout.GetButtonByTag("btnMove");
                if (btn != null)
                {
                    btn.btnState = buttonState.Off;
                }
            }
            //attack button
            if (currentCombatMode.Equals("attack"))
            {
                btn = combatUiLayout.GetButtonByTag("btnAttack");
                if (btn != null)
                {
                    btn.btnState = buttonState.On;
                }
            }
            else
            {
                btn = combatUiLayout.GetButtonByTag("btnAttack");
                if (btn != null)
                {
                    btn.btnState = buttonState.Normal;
                }
            }
            //cast button
            if (currentCombatMode.Equals("cast"))
            {
                btn = combatUiLayout.GetButtonByTag("btnCast");
                if (btn != null)
                {
                    btn.btnState = buttonState.On;
                }
            }
            else
            {
                btn = combatUiLayout.GetButtonByTag("btnCast");
                if (btn != null)
                {
                    btn.btnState = buttonState.Normal;
                }
            }

            combatUiLayout.Draw();            
        }
        public void drawCombatMap()
	    {
            if (mod.useAllTileSystem)
            {
                //row = y
                //col = x
                if (mod.currentEncounter.UseMapImage)
                {
                    /*
                    int sqrsizeW = mapBitmap.PixelSize.Width / this.mod.currentEncounter.MapSizeX;
                    int sqrsizeH = mapBitmap.PixelSize.Height / this.mod.currentEncounter.MapSizeY;
                    IbRect src = new IbRect(UpperLeftSquare.X * sqrsizeW, UpperLeftSquare.Y * sqrsizeH, sqrsizeW * 9, sqrsizeH * 9);
                    IbRect dst = new IbRect(0 + gv.oXshift + mapStartLocXinPixels, 0, gv.squareSize * 9, gv.squareSize * 9);
                    gv.DrawBitmap(mapBitmap, src, dst);
                    */
                    int bmpWidth = mapBitmap.PixelSize.Width;
                    int bmpHeight = mapBitmap.PixelSize.Height;
                    int sqrsizeW = mapBitmap.PixelSize.Width / this.mod.currentEncounter.MapSizeX;
                    int sqrsizeH = mapBitmap.PixelSize.Height / this.mod.currentEncounter.MapSizeY;
                    int dstX = -(UpperLeftSquare.X * gv.squareSize);
                    int dstY = -(UpperLeftSquare.Y * gv.squareSize);
                    int dstWidth = (int)(bmpWidth * 2 * gv.screenDensity); //assumes squares are 50x50 in this image
                    int dstHeight = (int)(bmpHeight * 2 * gv.screenDensity); //assumes squares are 50x50 in this image

                    IbRect src = new IbRect(0, 0, bmpWidth, bmpHeight);
                    //IbRect dst = new IbRect(dstX + gv.oXshift + mapStartLocXinPixels, dstY, dstWidth, dstHeight);
                    IbRect dst = new IbRect(dstX + gv.oXshift + mapStartLocXinPixels, dstY, dstWidth, dstHeight);
                    gv.DrawBitmap(mapBitmap, src, dst);

                    //draw grid
                    if (mod.com_showGrid)
                    {
                        src = new IbRect(0, 0, gv.squareSizeInPixels / 2, gv.squareSizeInPixels / 2);
                        dst = new IbRect(0 + mapStartLocXinPixels, 0, gv.squareSize, gv.squareSize);
                        for (int x = UpperLeftSquare.X; x < this.mod.currentEncounter.MapSizeX; x++)
                        {
                            for (int y = UpperLeftSquare.Y; y < this.mod.currentEncounter.MapSizeY; y++)
                            {
                                if (!IsInVisibleCombatWindow(x, y))
                                {
                                    continue;
                                }

                                int tlX = ((x - UpperLeftSquare.X) * gv.squareSize) + gv.oXshift + mapStartLocXinPixels;
                                int tlY = (y - UpperLeftSquare.Y) * gv.squareSize;
                                int brX = gv.squareSize;
                                int brY = gv.squareSize;

                                dst = new IbRect(tlX, tlY, brX, brY);
                                if (mod.currentEncounter.encounterTiles[y * mod.currentEncounter.MapSizeX + x].LoSBlocked)
                                {
                                    gv.DrawBitmap(gv.cc.losBlocked, src, dst);
                                }
                                if (mod.currentEncounter.encounterTiles[y * mod.currentEncounter.MapSizeX + x].Walkable != true)
                                {
                                    gv.DrawBitmap(gv.cc.walkBlocked, src, dst);
                                }
                                else
                                {
                                    gv.DrawBitmap(gv.cc.walkPass, src, dst);
                                }
                                if ((pf.values != null) && (mod.debugMode))
                                {
                                    //gv.DrawText(pf.values[x, y].ToString(), (x - UpperLeftSquare.X) * gv.squareSize + gv.oXshift + mapStartLocXinPixels, (y - UpperLeftSquare.Y) * gv.squareSize);
                                }
                            }
                        }
                    }
                }
                else //using tiles
                {
                    int minX = UpperLeftSquare.X - 5;
                    if (minX < 0) { minX = 0; }
                    int minY = UpperLeftSquare.Y - 5;
                    if (minY < 0) { minY = 0; }
                    int maxX = UpperLeftSquare.X + gv.playerOffsetX + gv.playerOffsetX + 1;
                    if (maxX > this.mod.currentEncounter.MapSizeX) { maxX = this.mod.currentEncounter.MapSizeX; }
                    int maxY = UpperLeftSquare.Y + gv.playerOffsetY + gv.playerOffsetY + 1;
                    if (maxY > this.mod.currentEncounter.MapSizeY) { maxY = this.mod.currentEncounter.MapSizeY; }

                    #region Draw Layer1
                    for (int x = minX; x < maxX; x++)
                    {
                        for (int y = minY; y < maxY; y++)
                        {
                            TileEnc tile = mod.currentEncounter.encounterTiles[y * mod.currentEncounter.MapSizeX + x];
                            try
                            {
                                //insert1                        
                                bool tileBitmapIsLoadedAlready = false;
                                int indexOfLoadedTile = -1;
                                for (int i = 0; i < gv.mod.loadedTileBitmapsNames.Count; i++)
                                {
                                    if (gv.mod.loadedTileBitmapsNames[i] == tile.Layer1Filename)
                                    {
                                        tileBitmapIsLoadedAlready = true;
                                        indexOfLoadedTile = i;
                                        break;
                                    }
                                }

                                //insert2
                                if (!tileBitmapIsLoadedAlready)
                                {
                                    gv.mod.loadedTileBitmapsNames.Add(tile.Layer1Filename);
                                    tile.tileBitmap1 = gv.cc.LoadBitmap(tile.Layer1Filename);
                                    gv.mod.loadedTileBitmaps.Add(tile.tileBitmap1);

                                    IbRect srcLyr = getSourceIbRect(
                                    x,
                                    y,
                                    UpperLeftSquare.X,
                                    UpperLeftSquare.Y,
                                    tile.tileBitmap1.PixelSize.Width,
                                    tile.tileBitmap1.PixelSize.Height);
                                    if (srcLyr != null)
                                    {
                                        int shiftY = srcLyr.Top / gv.squareSizeInPixels;
                                        int shiftX = srcLyr.Left / gv.squareSizeInPixels;
                                        int tlX = ((x - UpperLeftSquare.X + shiftX) * gv.squareSize) + gv.oXshift + mapStartLocXinPixels;
                                        int tlY = (y - UpperLeftSquare.Y + shiftY) * gv.squareSize;
                                        float scalerX = srcLyr.Width / 100;
                                        float scalerY = srcLyr.Height / 100;
                                        int brX = (int)(gv.squareSize * scalerX);
                                        int brY = (int)(gv.squareSize * scalerY);
                                        IbRect dstLyr = new IbRect(tlX, tlY, brX, brY);
                                        gv.DrawBitmap(tile.tileBitmap1, srcLyr, dstLyr);
                                    }
                                }
                                else
                                {
                                    IbRect srcLyr = getSourceIbRect(
                                    x,
                                    y,
                                    UpperLeftSquare.X,
                                    UpperLeftSquare.Y,
                                    gv.mod.loadedTileBitmaps[indexOfLoadedTile].PixelSize.Width,
                                    gv.mod.loadedTileBitmaps[indexOfLoadedTile].PixelSize.Height);
                                    if (srcLyr != null)
                                    {
                                        int shiftY = srcLyr.Top / gv.squareSizeInPixels;
                                        int shiftX = srcLyr.Left / gv.squareSizeInPixels;
                                        int tlX = ((x - UpperLeftSquare.X + shiftX) * gv.squareSize) + gv.oXshift + mapStartLocXinPixels;
                                        int tlY = (y - UpperLeftSquare.Y + shiftY) * gv.squareSize;
                                        float scalerX = srcLyr.Width / 100;
                                        float scalerY = srcLyr.Height / 100;
                                        int brX = (int)(gv.squareSize * scalerX);
                                        int brY = (int)(gv.squareSize * scalerY);
                                        IbRect dstLyr = new IbRect(tlX, tlY, brX, brY);
                                        gv.DrawBitmap(gv.mod.loadedTileBitmaps[indexOfLoadedTile], srcLyr, dstLyr);
                                    }

                                }
                            }
                            catch
                            { }
                        }
                    }
                    #endregion

                    #region Draw Layer2
                    for (int x = minX; x < maxX; x++)
                    {
                        for (int y = minY; y < maxY; y++)
                        {
                            TileEnc tile = mod.currentEncounter.encounterTiles[y * mod.currentEncounter.MapSizeX + x];
                            try
                            {
                                //insert1                        
                                bool tileBitmapIsLoadedAlready = false;
                                int indexOfLoadedTile = -1;
                                for (int i = 0; i < gv.mod.loadedTileBitmapsNames.Count; i++)
                                {
                                    if (gv.mod.loadedTileBitmapsNames[i] == tile.Layer2Filename)
                                    {
                                        tileBitmapIsLoadedAlready = true;
                                        indexOfLoadedTile = i;
                                        break;
                                    }
                                }

                                //insert2
                                if (!tileBitmapIsLoadedAlready)
                                {
                                    gv.mod.loadedTileBitmapsNames.Add(tile.Layer2Filename);
                                    tile.tileBitmap2 = gv.cc.LoadBitmap(tile.Layer2Filename);
                                    gv.mod.loadedTileBitmaps.Add(tile.tileBitmap2);

                                    IbRect srcLyr = getSourceIbRect(
                                    x,
                                    y,
                                    UpperLeftSquare.X,
                                    UpperLeftSquare.Y,
                                    tile.tileBitmap2.PixelSize.Width,
                                    tile.tileBitmap2.PixelSize.Height);
                                    if (srcLyr != null)
                                    {
                                        int shiftY = srcLyr.Top / gv.squareSizeInPixels;
                                        int shiftX = srcLyr.Left / gv.squareSizeInPixels;
                                        int tlX = ((x - UpperLeftSquare.X + shiftX) * gv.squareSize) + gv.oXshift + mapStartLocXinPixels;
                                        int tlY = (y - UpperLeftSquare.Y + shiftY) * gv.squareSize;
                                        float scalerX = srcLyr.Width / 100;
                                        float scalerY = srcLyr.Height / 100;
                                        int brX = (int)(gv.squareSize * scalerX);
                                        int brY = (int)(gv.squareSize * scalerY);
                                        IbRect dstLyr = new IbRect(tlX, tlY, brX, brY);
                                        gv.DrawBitmap(tile.tileBitmap2, srcLyr, dstLyr);
                                    }
                                }
                                else
                                {
                                    IbRect srcLyr = getSourceIbRect(
                                    x,
                                    y,
                                    UpperLeftSquare.X,
                                    UpperLeftSquare.Y,
                                    gv.mod.loadedTileBitmaps[indexOfLoadedTile].PixelSize.Width,
                                    gv.mod.loadedTileBitmaps[indexOfLoadedTile].PixelSize.Height);
                                    if (srcLyr != null)
                                    {
                                        int shiftY = srcLyr.Top / gv.squareSizeInPixels;
                                        int shiftX = srcLyr.Left / gv.squareSizeInPixels;
                                        int tlX = ((x - UpperLeftSquare.X + shiftX) * gv.squareSize) + gv.oXshift + mapStartLocXinPixels;
                                        int tlY = (y - UpperLeftSquare.Y + shiftY) * gv.squareSize;
                                        float scalerX = srcLyr.Width / 100;
                                        float scalerY = srcLyr.Height / 100;
                                        int brX = (int)(gv.squareSize * scalerX);
                                        int brY = (int)(gv.squareSize * scalerY);
                                        IbRect dstLyr = new IbRect(tlX, tlY, brX, brY);
                                        gv.DrawBitmap(gv.mod.loadedTileBitmaps[indexOfLoadedTile], srcLyr, dstLyr);
                                    }

                                }
                            }
                            catch
                            { }
                        }
                    }
                    #endregion

                    #region Draw Layer3
                    for (int x = minX; x < maxX; x++)
                    {
                        for (int y = minY; y < maxY; y++)
                        {
                            TileEnc tile = mod.currentEncounter.encounterTiles[y * mod.currentEncounter.MapSizeX + x];
                            try
                            {
                                //insert1                        
                                bool tileBitmapIsLoadedAlready = false;
                                int indexOfLoadedTile = -1;
                                for (int i = 0; i < gv.mod.loadedTileBitmapsNames.Count; i++)
                                {
                                    if (gv.mod.loadedTileBitmapsNames[i] == tile.Layer3Filename)
                                    {
                                        tileBitmapIsLoadedAlready = true;
                                        indexOfLoadedTile = i;
                                        break;
                                    }
                                }

                                //insert2
                                if (!tileBitmapIsLoadedAlready)
                                {
                                    gv.mod.loadedTileBitmapsNames.Add(tile.Layer3Filename);
                                    tile.tileBitmap3 = gv.cc.LoadBitmap(tile.Layer3Filename);
                                    gv.mod.loadedTileBitmaps.Add(tile.tileBitmap3);

                                    IbRect srcLyr = getSourceIbRect(
                                    x,
                                    y,
                                    UpperLeftSquare.X,
                                    UpperLeftSquare.Y,
                                    tile.tileBitmap3.PixelSize.Width,
                                    tile.tileBitmap3.PixelSize.Height);
                                    if (srcLyr != null)
                                    {
                                        int shiftY = srcLyr.Top / gv.squareSizeInPixels;
                                        int shiftX = srcLyr.Left / gv.squareSizeInPixels;
                                        int tlX = ((x - UpperLeftSquare.X + shiftX) * gv.squareSize) + gv.oXshift + mapStartLocXinPixels;
                                        int tlY = (y - UpperLeftSquare.Y + shiftY) * gv.squareSize;
                                        float scalerX = srcLyr.Width / 100;
                                        float scalerY = srcLyr.Height / 100;
                                        int brX = (int)(gv.squareSize * scalerX);
                                        int brY = (int)(gv.squareSize * scalerY);
                                        IbRect dstLyr = new IbRect(tlX, tlY, brX, brY);
                                        gv.DrawBitmap(tile.tileBitmap3, srcLyr, dstLyr);
                                    }
                                }
                                else
                                {
                                    IbRect srcLyr = getSourceIbRect(
                                    x,
                                    y,
                                    UpperLeftSquare.X,
                                    UpperLeftSquare.Y,
                                    gv.mod.loadedTileBitmaps[indexOfLoadedTile].PixelSize.Width,
                                    gv.mod.loadedTileBitmaps[indexOfLoadedTile].PixelSize.Height);
                                    if (srcLyr != null)
                                    {
                                        int shiftY = srcLyr.Top / gv.squareSizeInPixels;
                                        int shiftX = srcLyr.Left / gv.squareSizeInPixels;
                                        int tlX = ((x - UpperLeftSquare.X + shiftX) * gv.squareSize) + gv.oXshift + mapStartLocXinPixels;
                                        int tlY = (y - UpperLeftSquare.Y + shiftY) * gv.squareSize;
                                        float scalerX = srcLyr.Width / 100;
                                        float scalerY = srcLyr.Height / 100;
                                        int brX = (int)(gv.squareSize * scalerX);
                                        int brY = (int)(gv.squareSize * scalerY);
                                        IbRect dstLyr = new IbRect(tlX, tlY, brX, brY);
                                        gv.DrawBitmap(gv.mod.loadedTileBitmaps[indexOfLoadedTile], srcLyr, dstLyr);
                                    }

                                }
                            }
                            catch
                            { }
                        }
                    }
                    #endregion

                    #region Draw Grid
                    //I brought the pix width and height of source back to normal
                    if (mod.com_showGrid)
                    {
                        for (int x = UpperLeftSquare.X; x < this.mod.currentEncounter.MapSizeX; x++)
                        {
                            for (int y = UpperLeftSquare.Y; y < this.mod.currentEncounter.MapSizeY; y++)
                            {
                                if (!IsInVisibleCombatWindow(x, y))
                                {
                                    continue;
                                }

                                TileEnc tile = mod.currentEncounter.encounterTiles[y * mod.currentEncounter.MapSizeX + x];

                                int tlX = ((x - UpperLeftSquare.X) * gv.squareSize) + gv.oXshift + mapStartLocXinPixels;
                                int tlY = (y - UpperLeftSquare.Y) * gv.squareSize;
                                int brX = gv.squareSize;
                                int brY = gv.squareSize;

                                IbRect srcGrid = new IbRect(0, 0, gv.squareSizeInPixels, gv.squareSizeInPixels);
                                IbRect dstGrid = new IbRect(tlX, tlY, brX, brY);
                                if (mod.currentEncounter.encounterTiles[y * mod.currentEncounter.MapSizeX + x].LoSBlocked)
                                {
                                    gv.DrawBitmap(gv.cc.losBlocked, srcGrid, dstGrid);
                                }
                                if (mod.currentEncounter.encounterTiles[y * mod.currentEncounter.MapSizeX + x].Walkable != true)
                                {
                                    gv.DrawBitmap(gv.cc.walkBlocked, srcGrid, dstGrid);
                                }
                                else
                                {
                                    gv.DrawBitmap(gv.cc.walkPass, srcGrid, dstGrid);
                                }
                            }
                        }
                    }
                    #endregion
                    #region Draw Pathfinding Numbers
                    for (int x = UpperLeftSquare.X; x < this.mod.currentEncounter.MapSizeX; x++)
                    {
                        for (int y = UpperLeftSquare.Y; y < this.mod.currentEncounter.MapSizeY; y++)
                        {
                            if (!IsInVisibleCombatWindow(x, y))
                            {
                                continue;
                            }
                            if ((pf.values != null) && (mod.debugMode))
                            {
                                //gv.DrawText(pf.values[x, y].ToString(), (x - UpperLeftSquare.X) * gv.squareSize + gv.oXshift + mapStartLocXinPixels, (y - UpperLeftSquare.Y) * gv.squareSize);
                            }
                        }
                    }
                    #endregion
                }

                if (gv.mod.useManualCombatCam)
                {
                    drawColumnOfBlack();
                    drawRowOfBlack();
                }
            }
            else //old system using single image background and no load tile images on demand
            {
                //row = y
                //col = x
                if (mod.currentEncounter.UseMapImage)
                {
                    #region background image
                    int bmpWidth = mapBitmap.PixelSize.Width;
                    int bmpHeight = mapBitmap.PixelSize.Height;
                    int sqrsizeW = mapBitmap.PixelSize.Width / this.mod.currentEncounter.MapSizeX;
                    int sqrsizeH = mapBitmap.PixelSize.Height / this.mod.currentEncounter.MapSizeY;
                    int dstX = -(UpperLeftSquare.X * gv.squareSize);
                    int dstY = -(UpperLeftSquare.Y * gv.squareSize);
                    int dstWidth = (int)(bmpWidth * 2 * gv.screenDensity); //assumes squares are 50x50 in this image
                    int dstHeight = (int)(bmpHeight * 2 * gv.screenDensity); //assumes squares are 50x50 in this image
                    
                    IbRect src = new IbRect(0, 0, bmpWidth, bmpHeight);
                    IbRect dst = new IbRect(dstX + gv.oXshift + mapStartLocXinPixels, dstY, dstWidth, dstHeight);
                    gv.DrawBitmap(mapBitmap, src, dst);

                    //int sqrsizeW = mapBitmap.PixelSize.Width / this.mod.currentEncounter.MapSizeX;
                    //int sqrsizeH = mapBitmap.PixelSize.Height / this.mod.currentEncounter.MapSizeY;
                    //IbRect src = new IbRect(UpperLeftSquare.X * sqrsizeW, UpperLeftSquare.Y * sqrsizeH, sqrsizeW * (gv.playerOffsetX + gv.playerOffsetX + 1), sqrsizeH * (gv.playerOffsetY + gv.playerOffsetY + 1));
                    //IbRect dst = new IbRect(0 + gv.oXshift + mapStartLocXinPixels, 0, gv.squareSize * (gv.playerOffsetX + gv.playerOffsetX + 1), gv.squareSize * (gv.playerOffsetY + gv.playerOffsetY + 1));
                    //gv.DrawBitmap(mapBitmap, src, dst);                    
                    #endregion
                }

                int minX = UpperLeftSquare.X - (gv.playerOffsetX + 1);
                if (minX < 0) { minX = 0; }
                int minY = UpperLeftSquare.Y - (gv.playerOffsetY + 1);
                if (minY < 0) { minY = 0; }
                int maxX = UpperLeftSquare.X + gv.playerOffsetX + gv.playerOffsetX + 1;
                if (maxX > this.mod.currentEncounter.MapSizeX) { maxX = this.mod.currentEncounter.MapSizeX; }
                int maxY = UpperLeftSquare.Y + gv.playerOffsetY + gv.playerOffsetY + 2;
                if (maxY > this.mod.currentEncounter.MapSizeY) { maxY = this.mod.currentEncounter.MapSizeY; }

                #region Draw Layer1
                for (int x = minX; x < maxX; x++)
                {
                    for (int y = minY; y < maxY; y++)
                    {
                        TileEnc tile = mod.currentEncounter.encounterTiles[y * mod.currentEncounter.MapSizeX + x];
                        IbRect srcLyr = getSourceIbRect(
                            x,
                            y,
                            UpperLeftSquare.X,
                            UpperLeftSquare.Y,
                            gv.cc.GetFromTileBitmapList(tile.Layer1Filename).PixelSize.Width,
                            gv.cc.GetFromTileBitmapList(tile.Layer1Filename).PixelSize.Height);

                        if (srcLyr != null)
                        {
                            int shiftY = srcLyr.Top / gv.squareSizeInPixels;
                            int shiftX = srcLyr.Left / gv.squareSizeInPixels;
                            int tlX = ((x - UpperLeftSquare.X + shiftX) * gv.squareSize) + gv.oXshift + mapStartLocXinPixels;
                            int tlY = (y - UpperLeftSquare.Y + shiftY) * gv.squareSize;
                            float scalerX = srcLyr.Width / 100;
                            float scalerY = srcLyr.Height / 100;
                            int brX = (int)(gv.squareSize * scalerX);
                            int brY = (int)(gv.squareSize * scalerY);
                            IbRect dstLyr = new IbRect(tlX, tlY, brX, brY);
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(tile.Layer1Filename), srcLyr, dstLyr, tile.Layer1Rotate, tile.Layer1Mirror, tile.Layer1Xshift, tile.Layer1Yshift, tile.Layer1Xscale, tile.Layer1Yscale);
                        }
                    }
                }
                #endregion
                #region Draw Layer2
                for (int x = minX; x < maxX; x++)
                {
                    for (int y = minY; y < maxY; y++)
                    {
                        TileEnc tile = mod.currentEncounter.encounterTiles[y * mod.currentEncounter.MapSizeX + x];
                        IbRect srcLyr = getSourceIbRect(
                            x,
                            y,
                            UpperLeftSquare.X,
                            UpperLeftSquare.Y,
                            gv.cc.GetFromTileBitmapList(tile.Layer2Filename).PixelSize.Width,
                            gv.cc.GetFromTileBitmapList(tile.Layer2Filename).PixelSize.Height);

                        if (srcLyr != null)
                        {
                            int shiftY = srcLyr.Top / gv.squareSizeInPixels;
                            int shiftX = srcLyr.Left / gv.squareSizeInPixels;
                            int tlX = ((x - UpperLeftSquare.X + shiftX) * gv.squareSize) + gv.oXshift + mapStartLocXinPixels;
                            int tlY = (y - UpperLeftSquare.Y + shiftY) * gv.squareSize;
                            float scalerX = srcLyr.Width / 100;
                            float scalerY = srcLyr.Height / 100;
                            int brX = (int)(gv.squareSize * scalerX);
                            int brY = (int)(gv.squareSize * scalerY);
                            IbRect dstLyr = new IbRect(tlX, tlY, brX, brY);
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(tile.Layer2Filename), srcLyr, dstLyr, tile.Layer2Rotate, tile.Layer2Mirror, tile.Layer2Xshift, tile.Layer2Yshift, tile.Layer2Xscale, tile.Layer2Yscale);
                        }
                    }
                }
                #endregion
                #region Draw Layer3
                for (int x = minX; x < maxX; x++)
                {
                    for (int y = minY; y < maxY; y++)
                    {
                        TileEnc tile = mod.currentEncounter.encounterTiles[y * mod.currentEncounter.MapSizeX + x];
                        IbRect srcLyr = getSourceIbRect(
                            x,
                            y,
                            UpperLeftSquare.X,
                            UpperLeftSquare.Y,
                            gv.cc.GetFromTileBitmapList(tile.Layer3Filename).PixelSize.Width,
                            gv.cc.GetFromTileBitmapList(tile.Layer3Filename).PixelSize.Height);

                        if (srcLyr != null)
                        {
                            int shiftY = srcLyr.Top / gv.squareSizeInPixels;
                            int shiftX = srcLyr.Left / gv.squareSizeInPixels;
                            int tlX = ((x - UpperLeftSquare.X + shiftX) * gv.squareSize) + gv.oXshift + mapStartLocXinPixels;
                            int tlY = (y - UpperLeftSquare.Y + shiftY) * gv.squareSize;
                            float scalerX = srcLyr.Width / 100;
                            float scalerY = srcLyr.Height / 100;
                            int brX = (int)(gv.squareSize * scalerX);
                            int brY = (int)(gv.squareSize * scalerY);
                            IbRect dstLyr = new IbRect(tlX, tlY, brX, brY);
                            gv.DrawBitmap(gv.cc.GetFromTileBitmapList(tile.Layer3Filename), srcLyr, dstLyr, tile.Layer3Rotate, tile.Layer3Mirror, tile.Layer3Xshift, tile.Layer3Yshift, tile.Layer3Xscale, tile.Layer3Yscale);
                        }
                    }
                }
                #endregion
                #region Draw Grid
                //I brought the pix width and height of source back to normal
                if (mod.com_showGrid)
                {
                    for (int x = UpperLeftSquare.X; x < this.mod.currentEncounter.MapSizeX; x++)
                    {
                        for (int y = UpperLeftSquare.Y; y < this.mod.currentEncounter.MapSizeY; y++)
                        {
                            if (!IsInVisibleCombatWindow(x, y))
                            {
                                continue;
                            }

                            TileEnc tile = mod.currentEncounter.encounterTiles[y * mod.currentEncounter.MapSizeX + x];

                            int tlX = ((x - UpperLeftSquare.X) * gv.squareSize) + gv.oXshift + mapStartLocXinPixels;
                            int tlY = (y - UpperLeftSquare.Y) * gv.squareSize;
                            int brX = gv.squareSize;
                            int brY = gv.squareSize;

                            IbRect srcGrid = new IbRect(0, 0, gv.squareSizeInPixels, gv.squareSizeInPixels);
                            IbRect dstGrid = new IbRect(tlX, tlY, brX, brY);
                            if (mod.currentEncounter.encounterTiles[y * mod.currentEncounter.MapSizeX + x].LoSBlocked)
                            {
                                gv.DrawBitmap(gv.cc.losBlocked, srcGrid, dstGrid);
                            }
                            if (mod.currentEncounter.encounterTiles[y * mod.currentEncounter.MapSizeX + x].Walkable != true)
                            {
                                gv.DrawBitmap(gv.cc.walkBlocked, srcGrid, dstGrid);
                            }
                            else
                            {
                                gv.DrawBitmap(gv.cc.walkPass, srcGrid, dstGrid);
                            }
                        }
                    }
                }
                #endregion
                #region Draw Pathfinding Numbers
                for (int x = UpperLeftSquare.X; x < this.mod.currentEncounter.MapSizeX; x++)
                {
                    for (int y = UpperLeftSquare.Y; y < this.mod.currentEncounter.MapSizeY; y++)
                    {
                        if (!IsInVisibleCombatWindow(x, y))
                        {
                            continue;
                        }
                        if ((pf.values != null) && (mod.debugMode))
                        {
                            //gv.DrawText(pf.values[x, y].ToString(), (x - UpperLeftSquare.X) * gv.squareSize + gv.oXshift + mapStartLocXinPixels, (y - UpperLeftSquare.Y) * gv.squareSize);
                        }
                    }
                }
                #endregion

                if (gv.mod.useManualCombatCam)
                {
                    drawColumnOfBlack();
                    drawRowOfBlack();
                }
            }
        }

        //XXXX
        public void drawColumnOfBlack()
        {
            if ((UpperLeftSquare.X <= 0) || (UpperLeftSquare.X >= (this.mod.currentEncounter.MapSizeX - 1 - (gv.playerOffsetX*2 + 1))))
            {
                int offset = 0;
                if (UpperLeftSquare.X <= 0)
                {
                    offset = UpperLeftSquare.X +1;
                    for (int y = -1; y < gv.playerOffsetY * 2 + 3; y++)
                    {
                        int tlX = -offset * gv.squareSize;
                        int tlY = y * gv.squareSize;
                        int brX = gv.squareSize;
                        int brY = gv.squareSize;
                        IbRect src = new IbRect(0, 0, gv.cc.black_tile.PixelSize.Width, gv.cc.black_tile.PixelSize.Height);
                        IbRect dst = new IbRect(tlX + mapStartLocXinPixels - (int)(brX * 0.1f), tlY - (int)(brY * 0.1f), (int)(brX * 1.4f), (int)(brY * 1.3f));
                        gv.DrawBitmap(gv.cc.black_tile2, src, dst);
                    }
                }

                if (UpperLeftSquare.X >= (this.mod.currentEncounter.MapSizeX - 1 - (gv.playerOffsetX * 2 + 1)))
                {
                    offset = ((this.mod.currentEncounter.MapSizeX - 1) - UpperLeftSquare.X +1)*-1;
                    //offset = gv.playerOffsetX * 2 + 1 + offset -1;
                    for (int y = -1; y < gv.playerOffsetY * 2 + 3; y++)
                    {
                        int tlX = -offset * gv.squareSize;
                        int tlY = y * gv.squareSize;
                        int brX = gv.squareSize;
                        int brY = gv.squareSize;
                        IbRect src = new IbRect(0, 0, gv.cc.black_tile.PixelSize.Width, gv.cc.black_tile.PixelSize.Height);
                        IbRect dst = new IbRect(tlX + mapStartLocXinPixels - (int)(brX * 0.1f), tlY - (int)(brY * 0.1f), (int)(brX * 1.4f), (int)(brY * 1.3f));
                        gv.DrawBitmap(gv.cc.black_tile2, src, dst);
                    }
                }
            }
        }

        public void drawRowOfBlack()
        {
            if ((UpperLeftSquare.Y <= 0) || (UpperLeftSquare.Y >= (this.mod.currentEncounter.MapSizeY - 1 - (gv.playerOffsetY*2+1))))
            {
                int offset = 0;
                if (UpperLeftSquare.Y <= 0)
                {
                    offset = UpperLeftSquare.Y + 1;
                    for (int x = -1; x < gv.playerOffsetX * 2 + 3; x++)
                    {
                        int tlX = x * gv.squareSize;
                        int tlY = -offset * gv.squareSize;
                        int brX = gv.squareSize;
                        int brY = gv.squareSize;
                        IbRect src = new IbRect(0, 0, gv.cc.black_tile.PixelSize.Width, gv.cc.black_tile.PixelSize.Height);
                        IbRect dst = new IbRect(tlX + mapStartLocXinPixels - (int)(brX * 0.1f), tlY - (int)(brY * 0.1f), (int)(brX * 1.4f), (int)(brY * 1.3f));

                        gv.DrawBitmap(gv.cc.black_tile2, src, dst);
                    }
                }
                
                if (UpperLeftSquare.Y >= (this.mod.currentEncounter.MapSizeY - 1 - (gv.playerOffsetY * 2 + 1)))
                {
                    offset = ((this.mod.currentEncounter.MapSizeY - 1) - UpperLeftSquare.Y + 1)*-1;
                    //offset = gv.playerOffsetY * 2 + 1 + offset - 1;
                    for (int x = -1; x < gv.playerOffsetX * 2 + 3; x++)
                    {
                        int tlX = x * gv.squareSize;
                        int tlY = -offset * gv.squareSize;
                        int brX = gv.squareSize;
                        int brY = gv.squareSize;
                        IbRect src = new IbRect(0, 0, gv.cc.black_tile.PixelSize.Width, gv.cc.black_tile.PixelSize.Height);
                        IbRect dst = new IbRect(tlX + mapStartLocXinPixels - (int)(brX * 0.1f), tlY - (int)(brY * 0.1f), (int)(brX * 1.4f), (int)(brY * 1.3f));

                        gv.DrawBitmap(gv.cc.black_tile2, src, dst);
                    }
                }
            }
        }


        //XXXX
        public IbRect getSourceIbRect(int xSqr, int ySqr, int UpperLeftXsqr, int UpperLeftYsqr, int tileWinPixels, int tileHinPixels)
        {
            IbRect src = new IbRect(0, 0, tileWinPixels, tileHinPixels);

            int tileWsqrs = tileWinPixels / gv.squareSizeInPixels;
            int tileHsqrs = tileHinPixels / gv.squareSizeInPixels;
            int BottomRightX = UpperLeftXsqr + gv.playerOffsetX + gv.playerOffsetX + 1;
            int BottomRightY = UpperLeftYsqr + gv.playerOffsetY + gv.playerOffsetY + 2;

            //left side
            int startX = UpperLeftXsqr - xSqr;
            if (startX < 0) { startX = 0; }
            //if startX >= tileW then it is off the map
            if (startX >= tileWsqrs) { return null; }

            //top side
            int startY = UpperLeftYsqr - ySqr;
            if (startY < 0) { startY = 0; }
            //if startY >= tileY then it is off the map
            if (startY >= tileHsqrs) { return null; }

            //right side
            int endX = BottomRightX - xSqr;
            if (endX > tileWsqrs) { endX = tileWsqrs; }
            //if endX <=0 then it is off the map
            if (endX <= 0) { return null; }

            //bottom side
            int endY = BottomRightY - ySqr;
            if (endY > tileHsqrs) { endY = tileHsqrs; }
            //if endY <=0 then it is off the map
            if (endY <= 0) { return null; }
            
            return new IbRect(startX * gv.squareSizeInPixels, startY * gv.squareSizeInPixels, (endX - startX) * gv.squareSizeInPixels, (endY - startY) * gv.squareSizeInPixels);            
        }
        public void drawCombatPlayers()
	    {
		    Player p = mod.playerList[currentPlayerIndex];
            if (IsInVisibleCombatWindow(p.combatLocX, p.combatLocY))
            {
                IbRect src = new IbRect(0, 0, gv.cc.turn_marker.PixelSize.Width, gv.cc.turn_marker.PixelSize.Width);
                IbRect dst = new IbRect(getPixelLocX(p.combatLocX), getPixelLocY(p.combatLocY), gv.squareSize, gv.squareSize);
                if (isPlayerTurn)
                {
                    gv.DrawBitmap(gv.cc.turn_marker, src, dst);
                }
            }
		    foreach (Player pc in mod.playerList)
		    {
                if (IsInVisibleCombatWindow(pc.combatLocX, pc.combatLocY))
                {
                    IbRect src = new IbRect(0, 0, pc.token.PixelSize.Width, pc.token.PixelSize.Width);
                    //check if drawing animation of player
                    if ((playerToAnimate != null) && (playerToAnimate == pc))
                    {
                        src = new IbRect(0, pc.token.PixelSize.Width, pc.token.PixelSize.Width, pc.token.PixelSize.Width);
                    }
                    IbRect dst = new IbRect(getPixelLocX(pc.combatLocX), getPixelLocY(pc.combatLocY), gv.squareSize, gv.squareSize);
                    gv.DrawBitmap(pc.token, src, dst, !pc.combatFacingLeft);
                    src = new IbRect(0, 0, pc.token.PixelSize.Width, pc.token.PixelSize.Width);
                    foreach (Effect ef in pc.effectsList)
                    {
                        Bitmap fx = gv.cc.LoadBitmap(ef.spriteFilename);
                        src = new IbRect(0, 0, fx.PixelSize.Width, fx.PixelSize.Width);
                        gv.DrawBitmap(fx, src, dst);
                        gv.cc.DisposeOfBitmap(ref fx);
                    }
                    if ((pc.isDead()) || (pc.isUnconcious()))
                    {
                        src = new IbRect(0, 0, gv.cc.pc_dead.PixelSize.Width, gv.cc.pc_dead.PixelSize.Width);
                        gv.DrawBitmap(gv.cc.pc_dead, src, dst);
                    }
                    if (pc.steathModeOn)
                    {
                        src = new IbRect(0, 0, gv.cc.pc_stealth.PixelSize.Width, gv.cc.pc_stealth.PixelSize.Width);
                        gv.DrawBitmap(gv.cc.pc_stealth, src, dst);
                    }
                    //PLAYER FACING
                    src = new IbRect(0, 0, gv.cc.facing1.PixelSize.Width, gv.cc.facing1.PixelSize.Height);
                    if (pc.combatFacing == 8) { gv.DrawBitmap(gv.cc.facing8, src, dst); }
                    else if (pc.combatFacing == 9) { gv.DrawBitmap(gv.cc.facing9, src, dst); }
                    else if (pc.combatFacing == 6) { gv.DrawBitmap(gv.cc.facing6, src, dst); }
                    else if (pc.combatFacing == 3) { gv.DrawBitmap(gv.cc.facing3, src, dst); }
                    else if (pc.combatFacing == 2) { gv.DrawBitmap(gv.cc.facing2, src, dst); }
                    else if (pc.combatFacing == 1) { gv.DrawBitmap(gv.cc.facing1, src, dst); }
                    else if (pc.combatFacing == 4) { gv.DrawBitmap(gv.cc.facing4, src, dst); }
                    else if (pc.combatFacing == 7) { gv.DrawBitmap(gv.cc.facing7, src, dst); }
                    else { } //didn't find one


                    if (showMoveOrder)
                    {
                        int mo = pc.moveOrder + 1;
                        drawText(getPixelLocX(pc.combatLocX), getPixelLocY(pc.combatLocY) - (int)gv.drawFontRegHeight, mo.ToString(), Color.White);
                    }
                }
		    }
	    }
        public void drawLosTrail()
	    {
		    Player p = mod.playerList[currentPlayerIndex];
		    if ((currentCombatMode.Equals("attack")) || (currentCombatMode.Equals("cast")))
		    {
                //Uses the Screen Pixel Locations
			    int endX = getPixelLocX(targetHighlightCenterLocation.X) + (gv.squareSize / 2);
			    int endY = getPixelLocY(targetHighlightCenterLocation.Y) + (gv.squareSize / 2) + gv.oYshift;                
			    int startX = getPixelLocX(p.combatLocX) + (gv.squareSize / 2);
			    int startY = getPixelLocY(p.combatLocY) + (gv.squareSize / 2) + gv.oYshift;
                //Uses the Map Pixel Locations
                int endX2 = targetHighlightCenterLocation.X * gv.squareSize + (gv.squareSize / 2);
                int endY2 = targetHighlightCenterLocation.Y * gv.squareSize + (gv.squareSize / 2);
                int startX2 = p.combatLocX * gv.squareSize + (gv.squareSize / 2);
                int startY2 = p.combatLocY * gv.squareSize + (gv.squareSize / 2);
			    
                //check if target is within attack distance, use green if true, red if false
			    if (isVisibleLineOfSight(new Coordinate(endX2,endY2), new Coordinate(startX2,startY2))) 
                {
                    drawVisibleLineOfSightTrail(new Coordinate(endX,endY), new Coordinate(startX,startY), Color.Lime, 2);                    
                }
			    else 
                {
                    drawVisibleLineOfSightTrail(new Coordinate(endX, endY), new Coordinate(startX, startY), Color.Red, 2); 
                }
		    }
	    }
	    public void drawMovingCombatCreatures()
	    {
		    if (mod.currentEncounter.encounterCreatureList.Count > 0)
		    {
                if (!isPlayerTurn)
                {
                    Creature cr = mod.currentEncounter.encounterCreatureList[creatureIndex];
                    if (IsInVisibleCombatWindow(cr.combatLocX, cr.combatLocY))
                    {
                        IbRectF src = new IbRectF(0, 0, gv.cc.turn_marker.PixelSize.Width, gv.cc.turn_marker.PixelSize.Height);
                        IbRectF dst = new IbRectF(getPixelLocX(cr.combatLocX), getPixelLocY(cr.combatLocY), gv.squareSize, gv.squareSize);
                        gv.DrawBitmap(gv.cc.turn_marker, src, dst);
                    }
                }
		    }
		    foreach (Creature crt in mod.currentEncounter.encounterCreatureList)
		    {

                //XXXXXXXXXXXXXXXXXXXXXXXX


                int randXInt = 0;
                int randYInt = 0;
                float randX = 0;
                float randY = 0;
                int decider = 0;
                int moveChance = 80;

                decider = gv.sf.RandInt(90);
                if ((decider == 1) && (crt.inactiveTimer == 0))
                {
                    crt.inactiveTimer += gv.sf.RandInt(2);
                }

                if (crt.inactiveTimer != 0)
                {
                    crt.inactiveTimer += gv.sf.RandInt(2);
                }

                if (crt.inactiveTimer > 100)
                {
                    crt.inactiveTimer = 0;
                }

                if ((gv.sf.RandInt(100) <= moveChance) && (crt.inactiveTimer == 0))
                {
                    randXInt = gv.sf.RandInt(100);
                    randX = (randXInt+75) / 250f;
                    if (!crt.goRight)
                    {
                        crt.straightLineDistanceX += randX;
                        randX = -1 * randX;
                        if (crt.straightLineDistanceX >= 1.5f*gv.pS)
                        {
                            crt.goRight = true;
                            crt.straightLineDistanceX = 0;
                        }

                    }
                    else if (crt.goRight)
                    {
                        crt.straightLineDistanceX += randX;
                        randX = randX;
                        if (crt.straightLineDistanceX >= 1.5f*gv.pS)
                        {
                            crt.goRight = false;
                            crt.straightLineDistanceX = 0;
                        }
                    }

                    randYInt = gv.sf.RandInt(100);
                    randY = (randYInt+75) / 250f;
                    if (!crt.goDown)
                    {
                        crt.straightLineDistanceY += randY;
                        randY = -1 * randY;
                        if (crt.straightLineDistanceY >= 1.5*gv.pS)
                        {
                            crt.goDown = true;
                            crt.straightLineDistanceY = 0;
                        }
                            
                    }
                    else if (crt.goDown)
                    {
                        crt.straightLineDistanceY += randY;
                        randY = randY;
                        if (crt.straightLineDistanceY >= 1.5*gv.pS)
                        {
                            crt.goDown = false;
                            crt.straightLineDistanceY = 0;
                        }
                    }
                    else
                    {
                        //decider = gv.sf.RandInt(2);
                        //if (decider == 1)
                        //{
                            //randY = -1 * randY;
                        //}
                    }

                    crt.roamDistanceX += randX;
                    crt.roamDistanceY += randY;
                }
               //IbRect dst = new IbRect((int)this.position.X, (int)(this.position.Y + randY), (int)((gv.squareSize * this.scaleX) + randX), (int)(gv.squareSize * this.scaleY));
           
         
                //XXXXXXXXXXXXXXXXXXXXXXXX
                if (!IsInVisibleCombatWindow(crt.combatLocX, crt.combatLocY))
                {
                    continue;
                }
			    IbRectF src = new IbRectF(0, 0, crt.token.PixelSize.Width, crt.token.PixelSize.Width);
			    if ((creatureToAnimate != null) && (creatureToAnimate == crt))
			    {
                    attackAnimationFrameCounter++;
                    int maxUsableCounterValue = (int)(crt.token.PixelSize.Height / 100f - 1);
                    if (attackAnimationFrameCounter > maxUsableCounterValue)
                    {
                        attackAnimationFrameCounter = maxUsableCounterValue;
                    }
                    src = new IbRectF(0, crt.token.PixelSize.Width * attackAnimationFrameCounter, crt.token.PixelSize.Width, crt.token.PixelSize.Width);
			    }
                IbRectF dst = new IbRectF(getPixelLocX(crt.combatLocX) + crt.roamDistanceX, getPixelLocY(crt.combatLocY) + crt.roamDistanceY, gv.squareSize, gv.squareSize);
                if (crt.token.PixelSize.Width > 100)
			    {
                    dst = new IbRectF(getPixelLocX(crt.combatLocX) - (gv.squareSize / 2) + crt.roamDistanceX, getPixelLocY(crt.combatLocY) - (gv.squareSize / 2) + crt.roamDistanceY, gv.squareSize * 2, gv.squareSize * 2);
			    }
			    gv.DrawBitmap(crt.token, src, dst, !crt.combatFacingLeft);
			    foreach (Effect ef in crt.cr_effectsList)
			    {
				    Bitmap fx = gv.cc.LoadBitmap(ef.spriteFilename);
                    src = new IbRectF(0, 0, fx.PixelSize.Width, fx.PixelSize.Width);
				    gv.DrawBitmap(fx, src, dst);
                    gv.cc.DisposeOfBitmap(ref fx);
                }
                //CREATURE FACING
                src = new IbRectF(0, 0, gv.cc.facing1.PixelSize.Width, gv.cc.facing1.PixelSize.Height);
                if (crt.combatFacing == 8) { gv.DrawBitmap(gv.cc.facing8, src, dst); }
                else if (crt.combatFacing == 9) { gv.DrawBitmap(gv.cc.facing9, src, dst); }
                else if (crt.combatFacing == 6) { gv.DrawBitmap(gv.cc.facing6, src, dst); }
                else if (crt.combatFacing == 3) { gv.DrawBitmap(gv.cc.facing3, src, dst); }
                else if (crt.combatFacing == 2) { gv.DrawBitmap(gv.cc.facing2, src, dst); }
                else if (crt.combatFacing == 1) { gv.DrawBitmap(gv.cc.facing1, src, dst); }
                else if (crt.combatFacing == 4) { gv.DrawBitmap(gv.cc.facing4, src, dst); }
                else if (crt.combatFacing == 7) { gv.DrawBitmap(gv.cc.facing7, src, dst); }
                else { } //didn't find one

                if (showMoveOrder)
                {
                    int mo = crt.moveOrder + 1;
                    drawText(getPixelLocX(crt.combatLocX), getPixelLocY(crt.combatLocY) - (int)gv.drawFontRegHeight, mo.ToString(), Color.White);
                }
		    }
	    }
        public void drawCombatCreatures()
        {
            if (mod.currentEncounter.encounterCreatureList.Count > 0)
            {
                if (!isPlayerTurn)
                {
                    Creature cr = mod.currentEncounter.encounterCreatureList[creatureIndex];
                    if (IsInVisibleCombatWindow(cr.combatLocX, cr.combatLocY))
                    {
                        IbRect src = new IbRect(0, 0, gv.cc.turn_marker.PixelSize.Width, gv.cc.turn_marker.PixelSize.Height);
                        IbRect dst = new IbRect(getPixelLocX(cr.combatLocX), getPixelLocY(cr.combatLocY), gv.squareSize, gv.squareSize);
                        gv.DrawBitmap(gv.cc.turn_marker, src, dst);
                    }
                }
            }
            foreach (Creature crt in mod.currentEncounter.encounterCreatureList)
            {
                if (!IsInVisibleCombatWindow(crt.combatLocX, crt.combatLocY))
                {
                    continue;
                }
                IbRect src = new IbRect(0, 0, crt.token.PixelSize.Width, crt.token.PixelSize.Width);
                if ((creatureToAnimate != null) && (creatureToAnimate == crt))
                {
                    attackAnimationFrameCounter++;
                    int maxUsableCounterValue = (int)(crt.token.PixelSize.Height / 100f - 1);
                    if (attackAnimationFrameCounter > maxUsableCounterValue)
                    {
                        attackAnimationFrameCounter = maxUsableCounterValue;
                    }
                    src = new IbRect(0, crt.token.PixelSize.Width * attackAnimationFrameCounter, crt.token.PixelSize.Width, crt.token.PixelSize.Width);
			    }

                IbRect dst = new IbRect(getPixelLocX(crt.combatLocX), getPixelLocY(crt.combatLocY), gv.squareSize, gv.squareSize);
                if (crt.token.PixelSize.Width > 100)
                {
                    dst = new IbRect(getPixelLocX(crt.combatLocX) - (gv.squareSize / 2), getPixelLocY(crt.combatLocY) - (gv.squareSize / 2), gv.squareSize * 2, gv.squareSize * 2);
                }
                gv.DrawBitmap(crt.token, src, dst, !crt.combatFacingLeft);
                foreach (Effect ef in crt.cr_effectsList)
                {
                    Bitmap fx = gv.cc.LoadBitmap(ef.spriteFilename);
                    src = new IbRect(0, 0, fx.PixelSize.Width, fx.PixelSize.Width);
                    gv.DrawBitmap(fx, src, dst);
                    gv.cc.DisposeOfBitmap(ref fx);
                }
                //CREATURE FACING
                src = new IbRect(0, 0, gv.cc.facing1.PixelSize.Width, gv.cc.facing1.PixelSize.Height);
                if (crt.combatFacing == 8) { gv.DrawBitmap(gv.cc.facing8, src, dst); }
                else if (crt.combatFacing == 9) { gv.DrawBitmap(gv.cc.facing9, src, dst); }
                else if (crt.combatFacing == 6) { gv.DrawBitmap(gv.cc.facing6, src, dst); }
                else if (crt.combatFacing == 3) { gv.DrawBitmap(gv.cc.facing3, src, dst); }
                else if (crt.combatFacing == 2) { gv.DrawBitmap(gv.cc.facing2, src, dst); }
                else if (crt.combatFacing == 1) { gv.DrawBitmap(gv.cc.facing1, src, dst); }
                else if (crt.combatFacing == 4) { gv.DrawBitmap(gv.cc.facing4, src, dst); }
                else if (crt.combatFacing == 7) { gv.DrawBitmap(gv.cc.facing7, src, dst); }
                else { } //didn't find one

                if (showMoveOrder)
                {
                    int mo = crt.moveOrder + 1;
                    drawText(getPixelLocX(crt.combatLocX), getPixelLocY(crt.combatLocY) - (int)gv.drawFontRegHeight, mo.ToString(), Color.White);
                }
            }
        }
	    public void drawTargetHighlight()
	    {
		    Player pc = mod.playerList[currentPlayerIndex];
		    if (currentCombatMode.Equals("attack"))
		    {
                Item it = mod.getItemByResRefForInfo(pc.MainHandRefs.resref);
                //if using ranged and have ammo, use ammo properties
                if ((mod.getItemByResRefForInfo(pc.MainHandRefs.resref).category.Equals("Ranged"))
                        && (!mod.getItemByResRefForInfo(pc.AmmoRefs.resref).name.Equals("none")))
                {
                    //ranged weapon with ammo
                    it = mod.getItemByResRefForInfo(pc.AmmoRefs.resref);
                }
                if (it == null)
                {
                    it = mod.getItemByResRefForInfo(pc.MainHandRefs.resref);
                }
                //set squares list
                gv.sf.CreateAoeSquaresList(pc, targetHighlightCenterLocation, it.aoeShape, it.AreaOfEffect);
                foreach (Coordinate coor in gv.sf.AoeSquaresList)
                {
                    if (!IsInVisibleCombatWindow(coor.X, coor.Y))
                    {
                        continue;
                    }
                    bool hl_green = true;
                    int endX2 = coor.X * gv.squareSize + (gv.squareSize / 2);
                    int endY2 = coor.Y * gv.squareSize + (gv.squareSize / 2);
                    int startX2 = targetHighlightCenterLocation.X * gv.squareSize + (gv.squareSize / 2);
                    int startY2 = targetHighlightCenterLocation.Y * gv.squareSize + (gv.squareSize / 2);

                    if (isVisibleLineOfSight(new Coordinate(endX2, endY2), new Coordinate(startX2, startY2)))
                    {
                        hl_green = true;
                    }
                    else
                    {
                        hl_green = false;
                    }
                    if ((coor.X == targetHighlightCenterLocation.X) && (coor.Y == targetHighlightCenterLocation.Y))
                    {
                        int startX3 = pc.combatLocX * gv.squareSize + (gv.squareSize / 2);
                        int startY3 = pc.combatLocY * gv.squareSize + (gv.squareSize / 2);
                        if ((isValidAttackTarget(pc)) && (isVisibleLineOfSight(new Coordinate(endX2, endY2), new Coordinate(startX3, startY3))))
                        {
                            hl_green = true;
                        }
                        else
                        {
                            hl_green = false;
                        }
                    }

                    int x = getPixelLocX(coor.X);
                    int y = getPixelLocY(coor.Y);
                    IbRect src = new IbRect(0, 0, gv.cc.highlight_green.PixelSize.Width, gv.cc.highlight_green.PixelSize.Height);
                    IbRect dst = new IbRect(x, y, gv.squareSize, gv.squareSize);
                    if (hl_green)
                    {
                        gv.DrawBitmap(gv.cc.highlight_green, src, dst);
                    }
                    else
                    {
                        gv.DrawBitmap(gv.cc.highlight_red, src, dst);
                    }
                }
            }
		    else if (currentCombatMode.Equals("cast"))
		    {
                //set squares list
                gv.sf.CreateAoeSquaresList(pc, targetHighlightCenterLocation, gv.cc.currentSelectedSpell.aoeShape, gv.cc.currentSelectedSpell.aoeRadius);
                foreach (Coordinate coor in gv.sf.AoeSquaresList)
                {
                    if (!IsInVisibleCombatWindow(coor.X,coor.Y))
                    {
                        continue;
                    }
                    bool hl_green = true;
                    int endX2 = coor.X * gv.squareSize + (gv.squareSize / 2);
                    int endY2 = coor.Y * gv.squareSize + (gv.squareSize / 2);
                    int startX2 = targetHighlightCenterLocation.X * gv.squareSize + (gv.squareSize / 2);
                    int startY2 = targetHighlightCenterLocation.Y * gv.squareSize + (gv.squareSize / 2);

                    if ((isValidCastTarget(pc)) && (isVisibleLineOfSight(new Coordinate(endX2, endY2), new Coordinate(startX2, startY2))))
                    {
                        hl_green = true;
                    }
                    else
                    {
                        hl_green = false;
                    }
                    if ((coor.X == targetHighlightCenterLocation.X) && (coor.Y == targetHighlightCenterLocation.Y))
                    {
                        int startX3 = pc.combatLocX * gv.squareSize + (gv.squareSize / 2);
                        int startY3 = pc.combatLocY * gv.squareSize + (gv.squareSize / 2);
                        if ((isValidCastTarget(pc)) && (isVisibleLineOfSight(new Coordinate(endX2, endY2), new Coordinate(startX3, startY3))))
                        {
                            hl_green = true;
                        }
                        else
                        {
                            hl_green = false;
                        }
                    }

                    int x = getPixelLocX(coor.X);
                    int y = getPixelLocY(coor.Y);
                    IbRect src = new IbRect(0, 0, gv.cc.highlight_green.PixelSize.Width, gv.cc.highlight_green.PixelSize.Height);
                    IbRect dst = new IbRect(x, y, gv.squareSize, gv.squareSize);
                    if (hl_green)
                    {
                        gv.DrawBitmap(gv.cc.highlight_green, src, dst);
                    }
                    else
                    {
                        gv.DrawBitmap(gv.cc.highlight_red, src, dst);
                    }
                }                
		    }
        }	            
        public void drawFloatyText()
	    {            
		    int txtH = (int)gv.drawFontRegHeight;
		
		    for (int x = -2; x <= 2; x++)
		    {
			    for (int y = -2; y <= 2; y++)
			    {
                    gv.DrawText(gv.cc.floatyText, gv.cc.floatyTextLoc.X + x, gv.cc.floatyTextLoc.Y + txtH + y, 1.0f, Color.Black);
                    gv.DrawText(gv.cc.floatyText2, gv.cc.floatyTextLoc.X + x, gv.cc.floatyTextLoc.Y + (txtH * 2) + y, 1.0f, Color.Black);
                    gv.DrawText(gv.cc.floatyText3, gv.cc.floatyTextLoc.X + x, gv.cc.floatyTextLoc.Y + (txtH * 3) + y, 1.0f, Color.Black);	
			    }
		    }		
		    gv.DrawText(gv.cc.floatyText, gv.cc.floatyTextLoc.X, gv.cc.floatyTextLoc.Y + txtH, 1.0f, Color.Yellow);
            gv.DrawText(gv.cc.floatyText2, gv.cc.floatyTextLoc.X, gv.cc.floatyTextLoc.Y + txtH * 2, 1.0f, Color.Yellow);
            gv.DrawText(gv.cc.floatyText3, gv.cc.floatyTextLoc.X, gv.cc.floatyTextLoc.Y + txtH * 3, 1.0f, Color.Yellow);            
	    }
	    public void drawHPText()
	    {		
		    if ((showHP) && (!animationsOn))
		    {
			    foreach (Creature crt in mod.currentEncounter.encounterCreatureList)
			    {
                    if (IsInVisibleCombatWindow(crt.combatLocX, crt.combatLocY))
                    {
                        drawText(getPixelLocX(crt.combatLocX), getPixelLocY(crt.combatLocY), crt.hp + "/" + crt.hpMax, Color.Red);
                    }
			    }
			    foreach (Player pc in mod.playerList)
			    {
                    if (IsInVisibleCombatWindow(pc.combatLocX, pc.combatLocY))
                    {
                        drawText(getPixelLocX(pc.combatLocX), getPixelLocY(pc.combatLocY), pc.hp + "/" + pc.hpMax, Color.Red);
                    }
			    }
		    }
	    }
	    public void drawSPText()
	    {		
		    if ((showSP) && (!animationsOn))
		    {
			    int txtH = (int)gv.drawFontRegHeight;
			    foreach (Creature crt in mod.currentEncounter.encounterCreatureList)
			    {
                    if (IsInVisibleCombatWindow(crt.combatLocX, crt.combatLocY))
                    {
                        drawText(getPixelLocX(crt.combatLocX), getPixelLocY(crt.combatLocY) + txtH, "sp: " + crt.sp, Color.Yellow);
                    }
			    }
			    foreach (Player pc in mod.playerList)
			    {
                    if (IsInVisibleCombatWindow(pc.combatLocX, pc.combatLocY))
                    {
                        drawText(getPixelLocX(pc.combatLocX), getPixelLocY(pc.combatLocY) + txtH, pc.sp + "/" + pc.spMax, Color.Yellow);
                    }
			    }
		    }
	    }
	    public void drawText(int xLoc, int yLoc, string text, Color colr)
	    {
		    int txtH = (int)gv.drawFontRegHeight;

		    for (int x = -2; x <= 2; x++)
		    {
			    for (int y = -2; y <= 2; y++)
			    {
                    gv.DrawText(text, xLoc + x, yLoc + txtH + y, 1.0f, Color.Black);
			    }
		    }		
		    gv.DrawText(text, xLoc, yLoc + txtH, 1.0f, colr);
	    }
        public void drawMiniText(int xLoc, int yLoc, string text, Color colr)
        {
            int txtH = (int)gv.drawFontRegHeight;

            for (int x = -2; x <= 2; x++)
            {
                for (int y = -2; y <= 2; y++)
                {
                    gv.DrawText(text, xLoc + x, yLoc + txtH + y, 0.5f, Color.Black);
                }
            }
            gv.DrawText(text, xLoc, yLoc + txtH, 0.5f, colr);
        }
        public void drawFloatyTextList()
	    {
		    if (floatyTextOn)
		    {
                int txtH = (int)gv.drawFontRegHeight;
					
			    foreach (FloatyText ft in gv.cc.floatyTextList)
			    {
				    for (int x = -2; x <= 2; x++)
				    {
					    for (int y = -2; y <= 2; y++)
					    {
                            gv.DrawText(ft.value, ft.location.X - (UpperLeftSquare.X * gv.squareSize) + x + mapStartLocXinPixels, ft.location.Y - (UpperLeftSquare.Y * gv.squareSize) + y, 1.0f, Color.Black);
					    }
				    }
                    Color colr = Color.Yellow;
				    if (ft.color.Equals("yellow"))
				    {				
					    colr = Color.Yellow;
				    }
				    else if (ft.color.Equals("blue"))
				    {
                        colr = Color.Blue;
				    }
				    else if (ft.color.Equals("green"))
				    {
                        colr = Color.Lime;
				    }
				    else
				    {
                        colr = Color.Red;
				    }
                    gv.DrawText(ft.value, ft.location.X - (UpperLeftSquare.X * gv.squareSize) + mapStartLocXinPixels, ft.location.Y - (UpperLeftSquare.Y * gv.squareSize), 1.0f, colr);
			    }
		    }
	    }
        public void drawOverlayTints()
        {
            IbRect src = new IbRect(0, 0, gv.cc.tint_sunset.PixelSize.Width, gv.cc.tint_sunset.PixelSize.Height);
            IbRect dst = new IbRect(gv.oXshift + mapStartLocXinPixels, 0, gv.squareSize * (gv.playerOffsetX + gv.playerOffsetX + 1), gv.squareSize * (gv.playerOffsetY + gv.playerOffsetY + 2));
            int dawn = 5 * 60;
            int sunrise = 6 * 60;
            int day = 7 * 60;
            int sunset = 17 * 60;
            int dusk = 18 * 60;
            int night = 20 * 60;
            int time = gv.mod.WorldTime % 1440;
            if ((time >= dawn) && (time < sunrise))
            {
                gv.DrawBitmap(gv.cc.tint_dawn, src, dst);
            }
            else if ((time >= sunrise) && (time < day))
            {
                gv.DrawBitmap(gv.cc.tint_sunrise, src, dst);
            }
            else if ((time >= day) && (time < sunset))
            {
                //no tint for day
            }
            else if ((time >= sunset) && (time < dusk))
            {
                gv.DrawBitmap(gv.cc.tint_sunset, src, dst);
            }
            else if ((time >= dusk) && (time < night))
            {
                gv.DrawBitmap(gv.cc.tint_dusk, src, dst);
            }
            else if ((time >= night) || (time < dawn))
            {
                gv.DrawBitmap(gv.cc.tint_night, src, dst);
            }

        }
        public void drawSprites()
        {
            foreach (Sprite spr in spriteList)
            {
                spr.Draw(gv);
            }
            if (animationsOn)
            {
                if (attackAnimationTimeElapsed >= attackAnimationLengthInMilliseconds)
                {
                    foreach (AnimationSequence seq in animationSeqStack)
                    {
                        foreach (Sprite spr in seq.AnimationSeq[0].SpriteGroup)
                        {
                            //just draw the group at the top of the stack, first in first
                            spr.Draw(gv);
                        }
                    }
                }
            }
        }
        #endregion

        #region Keyboard Input
        public void onKeyUp(Keys keyData)
        {
            if (keyData == Keys.M)
            {
                if (canMove)
                {
                    if (isPlayerTurn)
                    {
                        currentCombatMode = "move";
                        gv.screenType = "combat";
                    }
                }
            }
            else if (keyData == Keys.A)
            {
                if (isPlayerTurn)
                {
                    Player pc = mod.playerList[currentPlayerIndex];
                    currentCombatMode = "attack";
                    gv.screenType = "combat";
                    setTargetHighlightStartLocation(pc);
                }
            }
            else if (keyData == Keys.P)
            {
                if (isPlayerTurn)
                {
                    if (currentPlayerIndex > mod.playerList.Count - 1)
                    {
                        return;
                    }
                    gv.cc.partyScreenPcIndex = currentPlayerIndex;
                    gv.screenParty.resetPartyScreen();
                    gv.screenType = "combatParty";
                }
            }
            else if (keyData == Keys.I)
            {
                if (isPlayerTurn)
                {
                    gv.screenType = "combatInventory";
                    gv.screenInventory.resetInventory();
                }
            }
            else if (keyData == Keys.S)
            {
                if (isPlayerTurn)
                {
                    gv.screenType = "combat";
                    endPcTurn(false);
                }
            }
            else if (keyData == Keys.C)
            {
                if (isPlayerTurn)
                {
                    Player pc = mod.playerList[currentPlayerIndex];
                    if (pc.knownSpellsTags.Count > 0)
                    {
                        currentCombatMode = "castSelector";
                        gv.screenType = "combatCast";
                        gv.screenCastSelector.castingPlayerIndex = currentPlayerIndex;
                        spellSelectorIndex = 0;
                        setTargetHighlightStartLocation(pc);
                    }
                    else
                    {
                        //TODO Toast.makeText(gv.gameContext, "PC has no Spells", Toast.LENGTH_SHORT).show();
                    }
                }
            }
            else if (keyData == Keys.X)
            {
                foreach (IB2Panel pnl in combatUiLayout.panelList)
                {                    
                    //hides left
                    if (pnl.hidingXIncrement < 0)
                    {
                        if (pnl.currentLocX < pnl.shownLocX)
                        {
                            pnl.showing = true;
                        }
                        else
                        {
                            pnl.hiding = true;
                        }
                    }
                    //hides right
                    else if (pnl.hidingXIncrement > 0)
                    {
                        if (pnl.currentLocX > pnl.shownLocX)
                        {
                            pnl.showing = true;
                        }
                        else
                        {
                            pnl.hiding = true;
                        }
                    }
                    //hides down
                    else if (pnl.hidingYIncrement > 0)
                    {
                        if (pnl.currentLocY > pnl.shownLocY)
                        {
                            if ((pnl.tag.Equals("arrowPanel")) && (!showArrows)) //don't show arrows
                            {
                                continue;
                            }
                            pnl.showing = true;
                        }
                        else
                        {
                            pnl.hiding = true;
                        }
                    }
                    //hides up
                    else if (pnl.hidingYIncrement < 0)
                    {
                        if (pnl.currentLocY < pnl.shownLocY)
                        {
                            pnl.showing = true;
                        }
                        else
                        {
                            pnl.hiding = true;
                        }
                    }
                }
            }

            #region Move Map
            if (keyData == Keys.Up)
            {
                if (gv.mod.useManualCombatCam)
                {
                    if (UpperLeftSquare.Y > -gv.playerOffsetY)
                    {
                        UpperLeftSquare.Y--;
                    }
                    return;
                }
                else
                {
                    if (UpperLeftSquare.Y > 0)
                    {
                        UpperLeftSquare.Y--;
                    }
                    return;
                }
            }
            else if (keyData == Keys.Left)
            {
                if (gv.mod.useManualCombatCam)
                {
                    if (UpperLeftSquare.X > -gv.playerOffsetX)
                    {
                        UpperLeftSquare.X--;
                    }
                    return;
                }
                else
                {
                    if (UpperLeftSquare.X > 0)
                    {
                        UpperLeftSquare.X--;
                    }
                    return;
                }
            }
            else if (keyData == Keys.Down)
            {
                if (gv.mod.useManualCombatCam)
                {
                    if (UpperLeftSquare.Y < mod.currentEncounter.MapSizeY - gv.playerOffsetY - 1)
                    {
                        UpperLeftSquare.Y++;
                    }
                    return;
                }
                else
                {
                    if (UpperLeftSquare.Y < mod.currentEncounter.MapSizeY - gv.playerOffsetY - gv.playerOffsetY - 1)
                    {
                        UpperLeftSquare.Y++;
                    }
                    return;
                }
            }
            else if (keyData == Keys.Right)
            {
                if (gv.mod.useManualCombatCam)
                {
                    if (UpperLeftSquare.X < mod.currentEncounter.MapSizeX - gv.playerOffsetX - 1)
                    {
                        UpperLeftSquare.X++;
                    }
                    return;
                }
                else
                {
                    if (UpperLeftSquare.X < mod.currentEncounter.MapSizeX - gv.playerOffsetX - gv.playerOffsetX - 1)
                    {
                        UpperLeftSquare.X++;
                    }
                    return;
                }
            }
            #endregion
            #region Move PC mode
            if (currentCombatMode.Equals("move"))
            {
                Player pc = mod.playerList[currentPlayerIndex];
                if (keyData == Keys.NumPad7) 
                {
                    MoveUpLeft(pc);
                }
                else if (keyData == Keys.NumPad8)
                {
                    MoveUp(pc);
                }
                else if (keyData == Keys.NumPad9)
                {
                    MoveUpRight(pc);
                }
                else if (keyData == Keys.NumPad4)
                {
                    MoveLeft(pc);
                }
                else if (keyData == Keys.NumPad5)
                {
                    CenterScreenOnPC();
                }
                else if (keyData == Keys.NumPad6)
                {
                    MoveRight(pc);
                }
                else if (keyData == Keys.NumPad1)
                {
                    MoveDownLeft(pc);
                }
                else if (keyData == Keys.NumPad2)
                {
                    MoveDown(pc);
                }
                else if (keyData == Keys.NumPad3)
                {
                    MoveDownRight(pc);
                }
                return;
            }
            #endregion
            #region Move Targeting Mode
            if (currentCombatMode.Equals("attack"))
            {
                Player pc = mod.playerList[currentPlayerIndex];
                if (keyData == Keys.NumPad5)
                {
                    TargetAttackPressed(pc);
                    return;
                }                
            }
            if (currentCombatMode.Equals("cast"))
            {
                Player pc = mod.playerList[currentPlayerIndex];
                if (keyData == Keys.NumPad5)
                {
                    TargetCastPressed(pc);
                    return;
                }                
            }
            if ((currentCombatMode.Equals("attack")) || (currentCombatMode.Equals("cast")))
            {
                if (keyData == Keys.NumPad7)
                {
                    MoveTargetHighlight(7);
                }
                else if (keyData == Keys.NumPad8)
                {
                    MoveTargetHighlight(8);
                }
                else if (keyData == Keys.NumPad9)
                {
                    MoveTargetHighlight(9);
                }
                else if (keyData == Keys.NumPad4)
                {
                    MoveTargetHighlight(4);
                }
                else if (keyData == Keys.NumPad6)
                {
                    MoveTargetHighlight(6);
                }
                else if (keyData == Keys.NumPad1)
                {
                    MoveTargetHighlight(1);
                }
                else if (keyData == Keys.NumPad2)
                {
                    MoveTargetHighlight(2);
                }
                else if (keyData == Keys.NumPad3)
                {
                    MoveTargetHighlight(3);
                }
                return;
            }
            #endregion
        }
        #endregion

        #region Mouse Input
        /*public void onTouchCombatOld(MouseEventArgs e, MouseEventType.EventType eventType)
        {
            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                    int x = (int)e.X;
                    int y = (int)e.Y;

                    int gridx = (int)(e.X - gv.oXshift - mapStartLocXinPixels) / gv.squareSize;
                    int gridy = (int)(e.Y - (gv.squareSize / 2)) / gv.squareSize;

                    #region FloatyText
                    gv.cc.floatyText = "";
                    gv.cc.floatyText2 = "";
                    gv.cc.floatyText3 = "";
                    foreach (Creature crt in mod.currentEncounter.encounterCreatureList)
                    {
                        if ((crt.combatLocX == gridx + UpperLeftSquare.X) && (crt.combatLocY == gridy + UpperLeftSquare.Y))
                        {
                            gv.cc.floatyText = crt.cr_name;
                            gv.cc.floatyText2 = "HP:" + crt.hp + " SP:" + crt.sp;
                            gv.cc.floatyText3 = "AC:" + crt.AC + " " + crt.cr_status;
                            gv.cc.floatyTextLoc = new Coordinate(getPixelLocX(crt.combatLocX), getPixelLocY(crt.combatLocY));
                        }
                    }
                    foreach (Player pc in mod.playerList)
                    {
                        if ((pc.combatLocX == gridx + UpperLeftSquare.X) && (pc.combatLocY == gridy + UpperLeftSquare.Y))
                        {
                            string am = "";
                            ItemRefs itr = mod.getItemRefsInInventoryByResRef(pc.AmmoRefs.resref);
                            if (itr != null)
                            {
                                am = itr.quantity + "";
                            }
                            else
                            {
                                am = "";
                            }

                            gv.cc.floatyText = pc.name;
                            int actext = 0;
                            if (mod.ArmorClassAscending) { actext = pc.AC; }
                            else { actext = 20 - pc.AC; }
                            gv.cc.floatyText2 = "AC:" + actext + " " + pc.charStatus;
                            gv.cc.floatyText3 = "Ammo: " + am;
                            gv.cc.floatyTextLoc = new Coordinate(getPixelLocX(pc.combatLocX), getPixelLocY(pc.combatLocY));

                        }
                    }
                    #endregion
                    #region Toggles
                    if (tglHP.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        if (tglHP.toggleOn)
                        {
                            tglHP.toggleOn = false;
                        }
                        else
                        {
                            tglHP.toggleOn = true;
                        }
                    }
                    if (tglSP.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        if (tglSP.toggleOn)
                        {
                            tglSP.toggleOn = false;
                        }
                        else
                        {
                            tglSP.toggleOn = true;
                        }
                    }
                    if (tglMoveOrder.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        if (tglMoveOrder.toggleOn)
                        {
                            tglMoveOrder.toggleOn = false;
                        }
                        else
                        {
                            tglMoveOrder.toggleOn = true;
                        }
                    }
                    if (tglSpeed.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        
                        if (mod.combatAnimationSpeed == 100)
                        {
                            mod.combatAnimationSpeed = 50;
                            gv.cc.addLogText("lime", "combat speed: 2x");
                            gv.cc.DisposeOfBitmap(ref tglSpeed.ImgOff);
                            tglSpeed.ImgOff = gv.cc.LoadBitmap("tgl_speed_2");
                        }
                        else if (mod.combatAnimationSpeed == 50)
                        {
                            mod.combatAnimationSpeed = 25;
                            gv.cc.addLogText("lime", "combat speed: 4x");
                            gv.cc.DisposeOfBitmap(ref tglSpeed.ImgOff);
                            tglSpeed.ImgOff = gv.cc.LoadBitmap("tgl_speed_4");
                        }
                        else if (mod.combatAnimationSpeed == 25)
                        {
                            mod.combatAnimationSpeed = 10;
                            gv.cc.addLogText("lime", "combat speed: 10x");
                            gv.cc.DisposeOfBitmap(ref tglSpeed.ImgOff);
                            tglSpeed.ImgOff = gv.cc.LoadBitmap("tgl_speed_10");
                        }
                        else if (mod.combatAnimationSpeed == 10)
                        {
                            mod.combatAnimationSpeed = 100;
                            gv.cc.addLogText("lime", "combat speed: 1x");
                            gv.cc.DisposeOfBitmap(ref tglSpeed.ImgOff);
                            tglSpeed.ImgOff = gv.cc.LoadBitmap("tgl_speed_1");
                        }
                    }
                    if (gv.cc.tglSound.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        if (gv.cc.tglSound.toggleOn)
                        {
                            gv.cc.tglSound.toggleOn = false;
                            mod.playMusic = false;
                            gv.stopCombatMusic();
                            //addLogText("lime","Music Off");
                        }
                        else
                        {
                            gv.cc.tglSound.toggleOn = true;
                            mod.playMusic = true;
                            gv.startCombatMusic();
                            //addLogText("lime","Music On");
                        }
                    }
                    if (tglSoundFx.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        if (tglSoundFx.toggleOn)
                        {
                            tglSoundFx.toggleOn = false;
                            mod.playSoundFx = false;
                            //gv.stopCombatMusic();
                            //addLogText("lime","Music Off");
                        }
                        else
                        {
                            tglSoundFx.toggleOn = true;
                            mod.playSoundFx = true;
                            //gv.startCombatMusic();
                            //addLogText("lime","Music On");
                        }
                    }
                    if (tglGrid.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        if (tglGrid.toggleOn)
                        {
                            tglGrid.toggleOn = false;
                            mod.com_showGrid = false;
                        }
                        else
                        {
                            tglGrid.toggleOn = true;
                            mod.com_showGrid = true;
                        }
                    }
                    if (tglHelp.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        tutorialMessageCombat(true);
                    }
                    if ((tglKill.getImpact(x, y)) && (mod.debugMode))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        mod.currentEncounter.encounterCreatureList.Clear();
                        mod.currentEncounter.encounterCreatureRefsList.Clear();
                        checkEndEncounter();
                    }
                    #endregion

                    if (btnSwitchWeapon.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}

                        if (currentPlayerIndex > mod.playerList.Count - 1)
                        {
                            return;
                        }
                        gv.cc.partyScreenPcIndex = currentPlayerIndex;
                        gv.screenParty.resetPartyScreen();
                        gv.screenType = "combatParty";
                    }
                    break;
            }
            //if MoveMode(move), AttackMode(attack), CastMode(cast), or InfoMode(info)
            if (currentCombatMode.Equals("info"))
            {
                onTouchCombatInfo(e, eventType);
            }
            else if (currentCombatMode.Equals("move"))
            {
                onTouchCombatMove(e, eventType);
            }
            else if (currentCombatMode.Equals("attack"))
            {
                onTouchCombatAttack(e, eventType);
            }
            else if (currentCombatMode.Equals("castSelector"))
            {
                //onTouchCombatCastSelector(event);
            }
            else if (currentCombatMode.Equals("cast"))
            {
                onTouchCombatCast(e, eventType);
            }
            else
            {
                //info mode
            }
        }*/
        /*public void onTouchCombatInfo(MouseEventArgs e, MouseEventType.EventType eventType)
        {
            //TODOgv.cc.onTouchLog();
            Player pc = mod.playerList[currentPlayerIndex];

            btnMove.glowOn = false;
            gv.cc.btnInventory.glowOn = false;
            btnAttack.glowOn = false;
            btnCast.glowOn = false;
            btnSkipTurn.glowOn = false;
            //btnKill.glowOn = false;
            //btnCombatHelp.glowOn = false;

            //int eventAction = event.getAction();
            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)e.X;
                    int y = (int)e.Y;
                    if (btnMove.getImpact(x, y))
                    {
                        btnMove.glowOn = true;
                    }
                    else if (gv.cc.btnInventory.getImpact(x, y))
                    {
                        gv.cc.btnInventory.glowOn = true;
                    }
                    else if (btnAttack.getImpact(x, y))
                    {
                        btnAttack.glowOn = true;
                    }
                    else if (btnCast.getImpact(x, y))
                    {
                        btnCast.glowOn = true;
                    }
                    else if (btnSkipTurn.getImpact(x, y))
                    {
                        btnSkipTurn.glowOn = true;
                    }
                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)e.X;
                    y = (int)e.Y;

                    btnMove.glowOn = false;
                    gv.cc.btnInventory.glowOn = false;
                    btnAttack.glowOn = false;
                    btnCast.glowOn = false;
                    btnSkipTurn.glowOn = false;

                    //BUTTONS			
                    if (btnMove.getImpact(x, y))
                    {
                        if (canMove)
                        {
                            //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                            //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                            currentCombatMode = "move";
                            gv.screenType = "combat";
                        }
                    }
                    else if (gv.cc.btnInventory.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        gv.screenType = "combatInventory";
                        gv.screenInventory.resetInventory();
                    }
                    else if (btnAttack.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        currentCombatMode = "attack";
                        gv.screenType = "combat";
                        setTargetHighlightStartLocation(pc);
                    }
                    else if (btnCast.getImpact(x, y))
                    {
                        if (pc.knownSpellsTags.Count > 0)
                        {
                            //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                            //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                            currentCombatMode = "castSelector";
                            gv.screenType = "combatCast";
                            gv.screenCastSelector.castingPlayerIndex = currentPlayerIndex;
                            spellSelectorIndex = 0;
                            setTargetHighlightStartLocation(pc);
                        }
                        else
                        {
                            //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                            //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                            //TODO Toast.makeText(gv.gameContext, "PC has no Spells", Toast.LENGTH_SHORT).show();
                        }
                    }
                    else if (btnSkipTurn.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        gv.screenType = "combat";
                        endPcTurn(false);
                    }
                    break;
            }
        }*/
        /*public void onTouchCombatMove(MouseEventArgs e, MouseEventType.EventType eventType)
        {
            //gv.cc.onTouchLog();
            Player pc = mod.playerList[currentPlayerIndex];

            gv.cc.ctrlUpArrow.glowOn = false;
            gv.cc.ctrlDownArrow.glowOn = false;
            gv.cc.ctrlLeftArrow.glowOn = false;
            gv.cc.ctrlRightArrow.glowOn = false;
            gv.cc.ctrlUpRightArrow.glowOn = false;
            gv.cc.ctrlDownRightArrow.glowOn = false;
            gv.cc.ctrlUpLeftArrow.glowOn = false;
            gv.cc.ctrlDownLeftArrow.glowOn = false;
            btnMove.glowOn = false;
            gv.cc.btnInventory.glowOn = false;
            btnAttack.glowOn = false;
            btnCast.glowOn = false;
            btnSkipTurn.glowOn = false;

            //int eventAction = event.getAction();
            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)e.X;
                    int y = (int)e.Y;
                    if (gv.cc.ctrlUpArrow.getImpact(x, y))
                    {
                        gv.cc.ctrlUpArrow.glowOn = true;
                    }
                    else if (gv.cc.ctrlDownArrow.getImpact(x, y))
                    {
                        gv.cc.ctrlDownArrow.glowOn = true;
                    }
                    else if (gv.cc.ctrlLeftArrow.getImpact(x, y))
                    {
                        gv.cc.ctrlLeftArrow.glowOn = true;
                    }
                    else if (gv.cc.ctrlRightArrow.getImpact(x, y))
                    {
                        gv.cc.ctrlRightArrow.glowOn = true;
                    }
                    else if (gv.cc.ctrlUpRightArrow.getImpact(x, y))
                    {
                        gv.cc.ctrlUpRightArrow.glowOn = true;
                    }
                    else if (gv.cc.ctrlDownRightArrow.getImpact(x, y))
                    {
                        gv.cc.ctrlDownRightArrow.glowOn = true;
                    }
                    else if (gv.cc.ctrlUpLeftArrow.getImpact(x, y))
                    {
                        gv.cc.ctrlUpLeftArrow.glowOn = true;
                    }
                    else if (gv.cc.ctrlDownLeftArrow.getImpact(x, y))
                    {
                        gv.cc.ctrlDownLeftArrow.glowOn = true;
                    }
                    else if (btnMove.getImpact(x, y))
                    {
                        btnMove.glowOn = true;
                    }
                    else if (gv.cc.btnInventory.getImpact(x, y))
                    {
                        gv.cc.btnInventory.glowOn = true;
                    }
                    else if (btnAttack.getImpact(x, y))
                    {
                        btnAttack.glowOn = true;
                    }
                    else if (btnCast.getImpact(x, y))
                    {
                        btnCast.glowOn = true;
                    }
                    else if (btnSkipTurn.getImpact(x, y))
                    {
                        btnSkipTurn.glowOn = true;
                    }
                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)e.X;
                    y = (int)e.Y;

                    gv.cc.ctrlUpArrow.glowOn = false;
                    gv.cc.ctrlDownArrow.glowOn = false;
                    gv.cc.ctrlLeftArrow.glowOn = false;
                    gv.cc.ctrlRightArrow.glowOn = false;
                    gv.cc.ctrlUpRightArrow.glowOn = false;
                    gv.cc.ctrlDownRightArrow.glowOn = false;
                    gv.cc.ctrlUpLeftArrow.glowOn = false;
                    gv.cc.ctrlDownLeftArrow.glowOn = false;
                    btnMove.glowOn = false;
                    gv.cc.btnInventory.glowOn = false;
                    btnAttack.glowOn = false;
                    btnCast.glowOn = false;
                    btnSkipTurn.glowOn = false;

                    //TOUCH ON MAP AREA
                    int gridx = (int)(e.X - gv.oXshift - mapStartLocXinPixels) / gv.squareSize;
                    int gridy = (int)(e.Y - (gv.squareSize / 2)) / gv.squareSize;
                    //int gridx = (int)e.X / gv.squareSize - 4;
                    //int gridy = (int)(e.Y - (gv.squareSize / 2)) / gv.squareSize;

                    if (gridy < mod.currentEncounter.MapSizeY)
                    {
                        gv.cc.floatyText = "";
                        gv.cc.floatyText2 = "";
                        gv.cc.floatyText3 = "";
                        //Check for second tap so TARGET
                    }

                    //BUTTONS
                    if ((gv.cc.ctrlUpArrow.getImpact(x, y)) || ((gridx + UpperLeftSquare.X == pc.combatLocX) && (gridy + UpperLeftSquare.Y == pc.combatLocY - 1)))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        MoveUp(pc);
                    }
                    else if ((gv.cc.ctrlDownArrow.getImpact(x, y)) || ((gridx + UpperLeftSquare.X == pc.combatLocX) && (gridy + UpperLeftSquare.Y == pc.combatLocY + 1)))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        MoveDown(pc);
                    }
                    else if ((gv.cc.ctrlLeftArrow.getImpact(x, y)) || ((gridx + UpperLeftSquare.X == pc.combatLocX - 1) && (gridy + UpperLeftSquare.Y == pc.combatLocY)))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        MoveLeft(pc);
                    }
                    else if ((gv.cc.ctrlRightArrow.getImpact(x, y)) || ((gridx + UpperLeftSquare.X == pc.combatLocX + 1) && (gridy + UpperLeftSquare.Y == pc.combatLocY)))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        MoveRight(pc);
                    }
                    else if ((gv.cc.ctrlUpRightArrow.getImpact(x, y)) || ((gridx + UpperLeftSquare.X == pc.combatLocX + 1) && (gridy + UpperLeftSquare.Y == pc.combatLocY - 1)))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        MoveUpRight(pc);
                    }
                    else if ((gv.cc.ctrlDownRightArrow.getImpact(x, y)) || ((gridx + UpperLeftSquare.X == pc.combatLocX + 1) && (gridy + UpperLeftSquare.Y == pc.combatLocY + 1)))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        MoveDownRight(pc);
                    }
                    else if ((gv.cc.ctrlUpLeftArrow.getImpact(x, y)) || ((gridx + UpperLeftSquare.X == pc.combatLocX - 1) && (gridy + UpperLeftSquare.Y == pc.combatLocY - 1)))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        MoveUpLeft(pc);
                    }
                    else if ((gv.cc.ctrlDownLeftArrow.getImpact(x, y)) || ((gridx + UpperLeftSquare.X == pc.combatLocX - 1) && (gridy + UpperLeftSquare.Y == pc.combatLocY + 1)))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        MoveDownLeft(pc);
                    }
                    else if (btnMove.getImpact(x, y))
                    {
                        if (canMove)
                        {
                            //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                            //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                            currentCombatMode = "info";
                            gv.screenType = "combat";
                            //Toast.makeText(gameContext, "Move Mode", Toast.LENGTH_SHORT).show();
                        }
                    }
                    else if (gv.cc.btnInventory.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        gv.screenType = "combatInventory";
                        gv.screenInventory.resetInventory();
                        //Toast.makeText(gameContext, "Inventory Button", Toast.LENGTH_SHORT).show();
                    }
                    else if (btnAttack.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        currentCombatMode = "attack";
                        gv.screenType = "combat";
                        setTargetHighlightStartLocation(pc);
                    }
                    else if (btnCast.getImpact(x, y))
                    {
                        if (pc.knownSpellsTags.Count > 0)
                        {
                            //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                            //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                            currentCombatMode = "castSelector";
                            gv.screenType = "combatCast";
                            gv.screenCastSelector.castingPlayerIndex = currentPlayerIndex;
                            spellSelectorIndex = 0;
                            setTargetHighlightStartLocation(pc);
                        }
                        else
                        {
                            //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                            //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                            //TODO Toast.makeText(gv.gameContext, "PC has no Spells", Toast.LENGTH_SHORT).show();
                        }
                    }
                    else if (btnSkipTurn.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        gv.screenType = "combat";
                        endPcTurn(false);
                    }
                    break;
            }
        }*/
        /*public void onTouchCombatAttack(MouseEventArgs e, MouseEventType.EventType eventType)
        {
            Player pc = mod.playerList[currentPlayerIndex];

            gv.cc.ctrlUpArrow.glowOn = false;
            gv.cc.ctrlDownArrow.glowOn = false;
            gv.cc.ctrlLeftArrow.glowOn = false;
            gv.cc.ctrlRightArrow.glowOn = false;
            gv.cc.ctrlUpRightArrow.glowOn = false;
            gv.cc.ctrlDownRightArrow.glowOn = false;
            gv.cc.ctrlUpLeftArrow.glowOn = false;
            gv.cc.ctrlDownLeftArrow.glowOn = false;
            btnSelect.glowOn = false;
            btnMove.glowOn = false;
            gv.cc.btnInventory.glowOn = false;
            btnAttack.glowOn = false;
            btnCast.glowOn = false;
            btnSkipTurn.glowOn = false;

            //int eventAction = event.getAction();
            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)e.X;
                    int y = (int)e.Y;
                    if (gv.cc.ctrlUpArrow.getImpact(x, y))
                    {
                        gv.cc.ctrlUpArrow.glowOn = true;
                    }
                    else if (gv.cc.ctrlDownArrow.getImpact(x, y))
                    {
                        gv.cc.ctrlDownArrow.glowOn = true;
                    }
                    else if (gv.cc.ctrlLeftArrow.getImpact(x, y))
                    {
                        gv.cc.ctrlLeftArrow.glowOn = true;
                    }
                    else if (gv.cc.ctrlRightArrow.getImpact(x, y))
                    {
                        gv.cc.ctrlRightArrow.glowOn = true;
                    }
                    else if (gv.cc.ctrlUpRightArrow.getImpact(x, y))
                    {
                        gv.cc.ctrlUpRightArrow.glowOn = true;
                    }
                    else if (gv.cc.ctrlDownRightArrow.getImpact(x, y))
                    {
                        gv.cc.ctrlDownRightArrow.glowOn = true;
                    }
                    else if (gv.cc.ctrlUpLeftArrow.getImpact(x, y))
                    {
                        gv.cc.ctrlUpLeftArrow.glowOn = true;
                    }
                    else if (gv.cc.ctrlDownLeftArrow.getImpact(x, y))
                    {
                        gv.cc.ctrlDownLeftArrow.glowOn = true;
                    }
                    else if (btnSelect.getImpact(x, y))
                    {
                        btnSelect.glowOn = true;
                    }
                    else if (btnMove.getImpact(x, y))
                    {
                        btnMove.glowOn = true;
                    }
                    else if (gv.cc.btnInventory.getImpact(x, y))
                    {
                        gv.cc.btnInventory.glowOn = true;
                    }
                    else if (btnAttack.getImpact(x, y))
                    {
                        btnAttack.glowOn = true;
                    }
                    else if (btnCast.getImpact(x, y))
                    {
                        btnCast.glowOn = true;
                    }
                    else if (btnSkipTurn.getImpact(x, y))
                    {
                        btnSkipTurn.glowOn = true;
                    }
                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)e.X;
                    y = (int)e.Y;

                    gv.cc.ctrlUpArrow.glowOn = false;
                    gv.cc.ctrlDownArrow.glowOn = false;
                    gv.cc.ctrlLeftArrow.glowOn = false;
                    gv.cc.ctrlRightArrow.glowOn = false;
                    gv.cc.ctrlUpRightArrow.glowOn = false;
                    gv.cc.ctrlDownRightArrow.glowOn = false;
                    gv.cc.ctrlUpLeftArrow.glowOn = false;
                    gv.cc.ctrlDownLeftArrow.glowOn = false;
                    btnSelect.glowOn = false;
                    btnMove.glowOn = false;
                    gv.cc.btnInventory.glowOn = false;
                    btnAttack.glowOn = false;
                    btnCast.glowOn = false;
                    btnSkipTurn.glowOn = false;

                    //TOUCH ON MAP AREA
                    int gridx = ((int)(e.X - gv.oXshift - mapStartLocXinPixels) / gv.squareSize) + UpperLeftSquare.X;
                    int gridy = ((int)(e.Y - (gv.squareSize / 2)) / gv.squareSize) + UpperLeftSquare.Y;

                    if (IsInVisibleCombatWindow(gridx, gridy))
                    {
                        gv.cc.floatyText = "";
                        gv.cc.floatyText2 = "";
                        gv.cc.floatyText3 = "";
                        //Check for second tap so TARGET
                        if ((gridx == targetHighlightCenterLocation.X) && (gridy == targetHighlightCenterLocation.Y))
                        {
                            TargetAttackPressed(pc);                            
                        }
                        //targetHighlightCenterLocation.Y = gridy + UpperLeftSquare.Y;
                        //targetHighlightCenterLocation.X = gridx + UpperLeftSquare.X;
                        targetHighlightCenterLocation.Y = gridy;
                        targetHighlightCenterLocation.X = gridx;
                    }

                    //BUTTONS
                    if (gv.cc.ctrlUpArrow.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        MoveTargetHighlight(8);
                    }
                    else if (gv.cc.ctrlDownArrow.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        MoveTargetHighlight(2);
                    }
                    else if (gv.cc.ctrlLeftArrow.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        MoveTargetHighlight(4);
                    }
                    else if (gv.cc.ctrlRightArrow.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        MoveTargetHighlight(6);
                    }
                    else if (gv.cc.ctrlUpRightArrow.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        MoveTargetHighlight(9);
                    }
                    else if (gv.cc.ctrlDownRightArrow.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        MoveTargetHighlight(3);
                    }
                    else if (gv.cc.ctrlUpLeftArrow.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        MoveTargetHighlight(7);
                    }
                    else if (gv.cc.ctrlDownLeftArrow.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        MoveTargetHighlight(1);
                    }
                    else if (btnSelect.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        TargetAttackPressed(pc);                        
                    }
                    else if (btnMove.getImpact(x, y))
                    {
                        if (canMove)
                        {
                            //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                            //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                            currentCombatMode = "move";
                            gv.screenType = "combat";
                        }
                    }
                    else if (gv.cc.btnInventory.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        gv.screenType = "combatInventory";
                        gv.screenInventory.resetInventory();
                    }
                    else if (btnAttack.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        currentCombatMode = "info";
                        gv.screenType = "combat";
                        setTargetHighlightStartLocation(pc);
                    }
                    else if (btnCast.getImpact(x, y))
                    {
                        if (pc.knownSpellsTags.Count > 0)
                        {
                            //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                            //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                            currentCombatMode = "castSelector";
                            gv.screenType = "combatCast";
                            gv.screenCastSelector.castingPlayerIndex = currentPlayerIndex;
                            spellSelectorIndex = 0;
                            setTargetHighlightStartLocation(pc);
                        }
                        else
                        {
                            //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                            //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                            //TODO Toast.makeText(gv.gameContext, "PC has no Spells", Toast.LENGTH_SHORT).show();
                        }
                    }
                    else if (btnSkipTurn.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        endPcTurn(false);
                    }
                    break;
            }
        }*/
        /*public void onTouchCombatCast(MouseEventArgs e, MouseEventType.EventType eventType)
        {
            //gv.cc.onTouchLog();
            Player pc = mod.playerList[currentPlayerIndex];

            gv.cc.ctrlUpArrow.glowOn = false;
            gv.cc.ctrlDownArrow.glowOn = false;
            gv.cc.ctrlLeftArrow.glowOn = false;
            gv.cc.ctrlRightArrow.glowOn = false;
            gv.cc.ctrlUpRightArrow.glowOn = false;
            gv.cc.ctrlDownRightArrow.glowOn = false;
            gv.cc.ctrlUpLeftArrow.glowOn = false;
            gv.cc.ctrlDownLeftArrow.glowOn = false;
            btnSelect.glowOn = false;
            btnMove.glowOn = false;
            gv.cc.btnInventory.glowOn = false;
            btnAttack.glowOn = false;
            btnCast.glowOn = false;
            btnSkipTurn.glowOn = false;

            //int eventAction = event.getAction();
            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)e.X;
                    int y = (int)e.Y;
                    if (gv.cc.ctrlUpArrow.getImpact(x, y))
                    {
                        gv.cc.ctrlUpArrow.glowOn = true;
                    }
                    else if (gv.cc.ctrlDownArrow.getImpact(x, y))
                    {
                        gv.cc.ctrlDownArrow.glowOn = true;
                    }
                    else if (gv.cc.ctrlLeftArrow.getImpact(x, y))
                    {
                        gv.cc.ctrlLeftArrow.glowOn = true;
                    }
                    else if (gv.cc.ctrlRightArrow.getImpact(x, y))
                    {
                        gv.cc.ctrlRightArrow.glowOn = true;
                    }
                    else if (gv.cc.ctrlUpRightArrow.getImpact(x, y))
                    {
                        gv.cc.ctrlUpRightArrow.glowOn = true;
                    }
                    else if (gv.cc.ctrlDownRightArrow.getImpact(x, y))
                    {
                        gv.cc.ctrlDownRightArrow.glowOn = true;
                    }
                    else if (gv.cc.ctrlUpLeftArrow.getImpact(x, y))
                    {
                        gv.cc.ctrlUpLeftArrow.glowOn = true;
                    }
                    else if (gv.cc.ctrlDownLeftArrow.getImpact(x, y))
                    {
                        gv.cc.ctrlDownLeftArrow.glowOn = true;
                    }
                    else if (btnSelect.getImpact(x, y))
                    {
                        btnSelect.glowOn = true;
                    }
                    else if (btnMove.getImpact(x, y))
                    {
                        btnMove.glowOn = true;
                    }
                    else if (gv.cc.btnInventory.getImpact(x, y))
                    {
                        gv.cc.btnInventory.glowOn = true;
                    }
                    else if (btnAttack.getImpact(x, y))
                    {
                        btnAttack.glowOn = true;
                    }
                    else if (btnCast.getImpact(x, y))
                    {
                        btnCast.glowOn = true;
                    }
                    else if (btnSkipTurn.getImpact(x, y))
                    {
                        btnSkipTurn.glowOn = true;
                    }
                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)e.X;
                    y = (int)e.Y;

                    gv.cc.ctrlUpArrow.glowOn = false;
                    gv.cc.ctrlDownArrow.glowOn = false;
                    gv.cc.ctrlLeftArrow.glowOn = false;
                    gv.cc.ctrlRightArrow.glowOn = false;
                    gv.cc.ctrlUpRightArrow.glowOn = false;
                    gv.cc.ctrlDownRightArrow.glowOn = false;
                    gv.cc.ctrlUpLeftArrow.glowOn = false;
                    gv.cc.ctrlDownLeftArrow.glowOn = false;
                    btnSelect.glowOn = false;
                    btnMove.glowOn = false;
                    gv.cc.btnInventory.glowOn = false;
                    btnAttack.glowOn = false;
                    btnCast.glowOn = false;
                    btnSkipTurn.glowOn = false;

                    //TOUCH ON MAP AREA
                    int gridx = ((int)(e.X - gv.oXshift - mapStartLocXinPixels) / gv.squareSize) + UpperLeftSquare.X;
                    int gridy = ((int)(e.Y - (gv.squareSize / 2)) / gv.squareSize) + UpperLeftSquare.Y;

                    if (IsInVisibleCombatWindow(gridx, gridy))
                    //if (gridy < mod.currentEncounter.MapSizeY)
                    {
                        gv.cc.floatyText = "";
                        gv.cc.floatyText2 = "";
                        gv.cc.floatyText3 = "";
                        //Check for second tap so TARGET
                        if ((gridx == targetHighlightCenterLocation.X) && (gridy == targetHighlightCenterLocation.Y))
                        {
                            TargetCastPressed(pc);
                        }
                        targetHighlightCenterLocation.Y = gridy;
                        targetHighlightCenterLocation.X = gridx;
                    }

                    //BUTTONS
                    if (gv.cc.ctrlUpArrow.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        MoveTargetHighlight(8);
                    }
                    else if (gv.cc.ctrlDownArrow.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        MoveTargetHighlight(2);
                    }
                    else if (gv.cc.ctrlLeftArrow.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        MoveTargetHighlight(4);
                    }
                    else if (gv.cc.ctrlRightArrow.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        MoveTargetHighlight(6);
                    }
                    else if (gv.cc.ctrlUpRightArrow.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        MoveTargetHighlight(9);
                    }
                    else if (gv.cc.ctrlDownRightArrow.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        MoveTargetHighlight(3);
                    }
                    else if (gv.cc.ctrlUpLeftArrow.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        MoveTargetHighlight(7);
                    }
                    else if (gv.cc.ctrlDownLeftArrow.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        MoveTargetHighlight(1);
                    }
                    else if (btnSelect.getImpact(x, y))
                    {
                        TargetCastPressed(pc);
                        //Toast.makeText(gameContext, "Selected", Toast.LENGTH_SHORT).show();
                    }
                    else if (btnMove.getImpact(x, y))
                    {
                        if (canMove)
                        {
                            //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                            //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                            currentCombatMode = "move";
                            gv.screenType = "combat";
                            //Toast.makeText(gameContext, "Move Mode", Toast.LENGTH_SHORT).show();
                        }
                    }
                    else if (gv.cc.btnInventory.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        gv.screenType = "combatInventory";
                        gv.screenInventory.resetInventory();
                        //Toast.makeText(gameContext, "Inventory Button", Toast.LENGTH_SHORT).show();
                    }
                    else if (btnAttack.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        currentCombatMode = "attack";
                        gv.screenType = "combat";
                        setTargetHighlightStartLocation(pc);
                        //Toast.makeText(gameContext, "Attack Mode", Toast.LENGTH_SHORT).show();
                    }
                    else if (btnCast.getImpact(x, y))
                    {
                        if (pc.knownSpellsTags.Count > 0)
                        {
                            //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                            //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                            currentCombatMode = "castSelector";
                            gv.screenType = "combatCast";
                            gv.screenCastSelector.castingPlayerIndex = currentPlayerIndex;
                            spellSelectorIndex = 0;
                            setTargetHighlightStartLocation(pc);
                        }
                        else
                        {
                            //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                            //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                            //TODO Toast.makeText(gv.gameContext, "PC has no Spells", Toast.LENGTH_SHORT).show();
                        }
                    }
                    else if (btnSkipTurn.getImpact(x, y))
                    {
                        //if (mod.playButtonSounds) {gv.playSoundEffect(android.view.SoundEffectConstants.CLICK);}
                        //if (mod.playButtonHaptic) {gv.performHapticFeedback(android.view.HapticFeedbackConstants.VIRTUAL_KEY);}
                        endPcTurn(false);
                    }
                    break;
            }
        }*/

        public void onTouchCombat(MouseEventArgs e, MouseEventType.EventType eventType)
        {
            switch (eventType)
            {
                case MouseEventType.EventType.MouseDown:
                case MouseEventType.EventType.MouseMove:
                    int x = (int)e.X;
                    int y = (int)e.Y;

                    //NEW SYSTEM
                    combatUiLayout.setHover(x, y);

                    int gridx = (int)(e.X - gv.oXshift - mapStartLocXinPixels) / gv.squareSize;
                    int gridy = (int)(e.Y - (gv.squareSize / 2)) / gv.squareSize;

                    #region FloatyText
                    gv.cc.floatyText = "";
                    gv.cc.floatyText2 = "";
                    gv.cc.floatyText3 = "";
                    foreach (Creature crt in mod.currentEncounter.encounterCreatureList)
                    {
                        if ((crt.combatLocX == gridx + UpperLeftSquare.X) && (crt.combatLocY == gridy + UpperLeftSquare.Y))
                        {
                            gv.cc.floatyText = crt.cr_name;
                            gv.cc.floatyText2 = "HP:" + crt.hp + " SP:" + crt.sp;
                            gv.cc.floatyText3 = "AC:" + crt.AC + " " + crt.cr_status;
                            gv.cc.floatyTextLoc = new Coordinate(getPixelLocX(crt.combatLocX), getPixelLocY(crt.combatLocY));
                        }
                    }
                    foreach (Player pc1 in mod.playerList)
                    {
                        if ((pc1.combatLocX == gridx + UpperLeftSquare.X) && (pc1.combatLocY == gridy + UpperLeftSquare.Y))
                        {
                            string am = "";
                            ItemRefs itr = mod.getItemRefsInInventoryByResRef(pc1.AmmoRefs.resref);
                            if (itr != null)
                            {
                                am = itr.quantity + "";
                            }
                            else
                            {
                                am = "";
                            }

                            gv.cc.floatyText = pc1.name;
                            int actext = 0;
                            if (mod.ArmorClassAscending) { actext = pc1.AC; }
                            else { actext = 20 - pc1.AC; }
                            gv.cc.floatyText2 = "AC:" + actext + " " + pc1.charStatus;
                            gv.cc.floatyText3 = "Ammo: " + am;
                            gv.cc.floatyTextLoc = new Coordinate(getPixelLocX(pc1.combatLocX), getPixelLocY(pc1.combatLocY));

                        }
                    }
                    #endregion

                    break;

                case MouseEventType.EventType.MouseUp:
                    x = (int)e.X;
                    y = (int)e.Y;

                    Player pc = mod.playerList[currentPlayerIndex];

                    //NEW SYSTEM
                    string rtn = combatUiLayout.getImpact(x, y);
                    //gv.cc.addLogText("lime", "mouse down: " + rtn);

                    #region Toggles
                    if (rtn.Equals("tglHP"))
                    {
                        IB2ToggleButton tgl = combatUiLayout.GetToggleByTag(rtn);
                        if (tgl == null) { return; }
                        tgl.toggleOn = !tgl.toggleOn;
                        showHP = !showHP;
                    }
                    if (rtn.Equals("tglSP"))
                    {
                        IB2ToggleButton tgl = combatUiLayout.GetToggleByTag(rtn);
                        if (tgl == null) { return; }
                        tgl.toggleOn = !tgl.toggleOn;
                        showSP = !showSP;
                    }
                    if (rtn.Equals("tglMoveOrder"))
                    {
                        IB2ToggleButton tgl = combatUiLayout.GetToggleByTag(rtn);
                        if (tgl == null) { return; }
                        tgl.toggleOn = !tgl.toggleOn;
                        showMoveOrder = !showMoveOrder;
                    }
                    if (rtn.Equals("tglIniBar"))
                    {
                        IB2ToggleButton tgl = combatUiLayout.GetToggleByTag(rtn);
                        if (tgl == null) { return; }
                        tgl.toggleOn = !tgl.toggleOn;
                        showIniBar = !showIniBar;
                    }
                    if (rtn.Equals("tglSpeed"))
                    {
                        IB2ToggleButton tgl = combatUiLayout.GetToggleByTag(rtn);
                        if (tgl == null) { return; }

                        if (mod.combatAnimationSpeed == 100)
                        {
                            mod.combatAnimationSpeed = 50;
                            tgl.ImgOffFilename = "tgl_speed_2";
                            gv.cc.addLogText("lime", "combat speed: 2x");                            
                        }
                        else if (mod.combatAnimationSpeed == 50)
                        {
                            mod.combatAnimationSpeed = 25;
                            tgl.ImgOffFilename = "tgl_speed_4";
                            gv.cc.addLogText("lime", "combat speed: 4x");                            
                        }
                        else if (mod.combatAnimationSpeed == 25)
                        {
                            mod.combatAnimationSpeed = 10;
                            tgl.ImgOffFilename = "tgl_speed_10";
                            gv.cc.addLogText("lime", "combat speed: 10x");
                        }
                        else if (mod.combatAnimationSpeed == 10)
                        {
                            mod.combatAnimationSpeed = 100;
                            tgl.ImgOffFilename = "tgl_speed_1";
                            gv.cc.addLogText("lime", "combat speed: 1x");
                        }
                    }
                    if (rtn.Equals("tglSound"))
                    {
                        IB2ToggleButton tgl = combatUiLayout.GetToggleByTag(rtn);
                        if (tgl == null) { return; }
                        if (tgl.toggleOn)
                        {
                            tgl.toggleOn = false;
                            mod.playMusic = false;
                            //TODO gv.screenCombat.tglSoundFx.toggleOn = false;
                            gv.stopCombatMusic();
                            gv.cc.addLogText("lime", "Music Off");
                        }
                        else
                        {
                            tgl.toggleOn = true;
                            mod.playMusic = true;
                            //TODO gv.screenCombat.tglSoundFx.toggleOn = true;
                            gv.startCombatMusic();
                            gv.cc.addLogText("lime", "Music On");
                        }
                    }
                    if (rtn.Equals("tglSoundFx"))
                    {
                        IB2ToggleButton tgl = combatUiLayout.GetToggleByTag(rtn);
                        if (tgl == null) { return; }
                        if (tgl.toggleOn)
                        {
                            tgl.toggleOn = false;
                            mod.playSoundFx = false;
                            gv.cc.addLogText("lime", "SoundFX Off");
                        }
                        else
                        {
                            tgl.toggleOn = true;
                            mod.playSoundFx = true;
                            gv.cc.addLogText("lime", "SoundFX On");
                        }
                    }
                    if (rtn.Equals("tglGrid"))
                    {
                        IB2ToggleButton tgl = combatUiLayout.GetToggleByTag(rtn);
                        if (tgl == null) { return; }
                        if (tgl.toggleOn)
                        {
                            tgl.toggleOn = false;
                            mod.com_showGrid = false;
                        }
                        else
                        {
                            tgl.toggleOn = true;
                            mod.com_showGrid = true;
                        }
                    }
                    if (rtn.Equals("tglHelp"))
                    {
                        tutorialMessageCombat(true);
                    }
                    if ((rtn.Equals("tglKill")) && (mod.debugMode))
                    {
                        mod.currentEncounter.encounterCreatureList.Clear();
                        mod.currentEncounter.encounterCreatureRefsList.Clear();
                        checkEndEncounter();
                    }
                    #endregion

                    #region TOUCH ON MAP AREA
                    gridx = ((int)(e.X - gv.oXshift - mapStartLocXinPixels) / gv.squareSize) + UpperLeftSquare.X;
                    gridy = ((int)(e.Y - (gv.squareSize / 2)) / gv.squareSize) + UpperLeftSquare.Y;
                    int tappedSqrX = ((int)(e.X - gv.oXshift - mapStartLocXinPixels) / gv.squareSize);
                    int tappedSqrY = ((int)(e.Y - (gv.squareSize / 2)) / gv.squareSize);

                    if (IsInVisibleCombatWindow(gridx, gridy))
                    {
                        gv.cc.floatyText = "";
                        gv.cc.floatyText2 = "";
                        gv.cc.floatyText3 = "";
                        if ((currentCombatMode.Equals("attack")) || (currentCombatMode.Equals("cast")))
                        {
                            //Check for second tap so TARGET
                            if ((gridx == targetHighlightCenterLocation.X) && (gridy == targetHighlightCenterLocation.Y))
                            {
                                if (currentCombatMode.Equals("attack"))
                                {
                                    TargetAttackPressed(pc);
                                }
                                else if (currentCombatMode.Equals("cast"))
                                {
                                    TargetCastPressed(pc);
                                }
                            }
                            targetHighlightCenterLocation.Y = gridy;
                            targetHighlightCenterLocation.X = gridx;
                        }                        
                    }
                    #endregion
                   
                    #region BUTTONS
                    if ((rtn.Equals("ctrlUpArrow")) || ((tappedSqrX + UpperLeftSquare.X == pc.combatLocX) && (tappedSqrY + UpperLeftSquare.Y == pc.combatLocY - 1)))
                    {
                        if (isPlayerTurn)
                        {
                            if (currentCombatMode.Equals("move"))
                            {
                                MoveUp(pc);
                            }
                            else if ((currentCombatMode.Equals("attack")) || (currentCombatMode.Equals("cast")))
                            {
                                if (rtn.Equals("ctrlUpArrow")) //if clicked on square, don't move the highlight...only move for arrow button
                                {
                                    MoveTargetHighlight(8);
                                }
                            }
                        }                            
                    }
                    else if ((rtn.Equals("ctrlDownArrow")) || ((tappedSqrX + UpperLeftSquare.X == pc.combatLocX) && (tappedSqrY + UpperLeftSquare.Y == pc.combatLocY + 1)))
                    {
                        if (isPlayerTurn)
                        {
                            if (currentCombatMode.Equals("move"))
                            {
                                MoveDown(pc);
                            }
                            else if ((currentCombatMode.Equals("attack")) || (currentCombatMode.Equals("cast")))
                            {
                                if (rtn.Equals("ctrlDownArrow")) //if clicked on square, don't move the highlight...only move for arrow button
                                {
                                    MoveTargetHighlight(2);
                                }
                            }
                        }
                    }
                    else if ((rtn.Equals("ctrlLeftArrow")) || ((tappedSqrX + UpperLeftSquare.X == pc.combatLocX - 1) && (tappedSqrY + UpperLeftSquare.Y == pc.combatLocY)))
                    {
                        if (isPlayerTurn)
                        {
                            if (currentCombatMode.Equals("move"))
                            {
                                MoveLeft(pc);
                            }
                            else if ((currentCombatMode.Equals("attack")) || (currentCombatMode.Equals("cast")))
                            {
                                if (rtn.Equals("ctrlLeftArrow")) //if clicked on square, don't move the highlight...only move for arrow button
                                {
                                    MoveTargetHighlight(4);
                                }
                            }
                        }
                    }
                    else if ((rtn.Equals("ctrlRightArrow")) || ((tappedSqrX + UpperLeftSquare.X == pc.combatLocX + 1) && (tappedSqrY + UpperLeftSquare.Y == pc.combatLocY)))
                    {
                        if (isPlayerTurn)
                        {
                            if (currentCombatMode.Equals("move"))
                            {
                                MoveRight(pc);
                            }
                            else if ((currentCombatMode.Equals("attack")) || (currentCombatMode.Equals("cast")))
                            {
                                if (rtn.Equals("ctrlRightArrow")) //if clicked on square, don't move the highlight...only move for arrow button
                                {
                                    MoveTargetHighlight(6);
                                }
                            }
                        }
                    }
                    else if ((rtn.Equals("ctrlUpRightArrow")) || ((tappedSqrX + UpperLeftSquare.X == pc.combatLocX + 1) && (tappedSqrY + UpperLeftSquare.Y == pc.combatLocY - 1)))
                    {
                        if (isPlayerTurn)
                        {
                            if (currentCombatMode.Equals("move"))
                            {
                                MoveUpRight(pc);
                            }
                            else if ((currentCombatMode.Equals("attack")) || (currentCombatMode.Equals("cast")))
                            {
                                if (rtn.Equals("ctrlUpRightArrow")) //if clicked on square, don't move the highlight...only move for arrow button
                                {
                                    MoveTargetHighlight(9);
                                }
                            }
                        }
                    }
                    else if ((rtn.Equals("ctrlDownRightArrow")) || ((tappedSqrX + UpperLeftSquare.X == pc.combatLocX + 1) && (tappedSqrY + UpperLeftSquare.Y == pc.combatLocY + 1)))
                    {
                        if (isPlayerTurn)
                        {
                            if (currentCombatMode.Equals("move"))
                            {
                                MoveDownRight(pc);
                            }
                            else if ((currentCombatMode.Equals("attack")) || (currentCombatMode.Equals("cast")))
                            {
                                if (rtn.Equals("ctrlDownRightArrow")) //if clicked on square, don't move the highlight...only move for arrow button
                                {
                                    MoveTargetHighlight(3);
                                }
                            }
                        }
                    }
                    else if ((rtn.Equals("ctrlUpLeftArrow")) || ((tappedSqrX + UpperLeftSquare.X == pc.combatLocX - 1) && (tappedSqrY + UpperLeftSquare.Y == pc.combatLocY - 1)))
                    {
                        if (isPlayerTurn)
                        {
                            if (currentCombatMode.Equals("move"))
                            {
                                MoveUpLeft(pc);
                            }
                            else if ((currentCombatMode.Equals("attack")) || (currentCombatMode.Equals("cast")))
                            {
                                if (rtn.Equals("ctrlUpLeftArrow")) //if clicked on square, don't move the highlight...only move for arrow button
                                {
                                    MoveTargetHighlight(7);
                                }
                            }
                        }
                    }
                    else if ((rtn.Equals("ctrlDownLeftArrow")) || ((tappedSqrX + UpperLeftSquare.X == pc.combatLocX - 1) && (tappedSqrY + UpperLeftSquare.Y == pc.combatLocY + 1)))
                    {
                        if (isPlayerTurn)
                        {
                            if (currentCombatMode.Equals("move"))
                            {
                                MoveDownLeft(pc);
                            }
                            else if ((currentCombatMode.Equals("attack")) || (currentCombatMode.Equals("cast")))
                            {
                                if (rtn.Equals("ctrlDownLeftArrow")) //if clicked on square, don't move the highlight...only move for arrow button
                                {
                                    MoveTargetHighlight(1);
                                }
                            }
                        }
                    }
                    else if (rtn.Equals("btnSwitchWeapon"))
                    {
                        if (isPlayerTurn)
                        {
                            if (currentPlayerIndex > mod.playerList.Count - 1)
                            {
                                return;
                            }
                            gv.cc.partyScreenPcIndex = currentPlayerIndex;
                            gv.screenParty.resetPartyScreen();
                            gv.screenType = "combatParty";
                        }
                    }
                    else if (rtn.Equals("btnMove"))
                    {
                        if (canMove)
                        {
                            if (currentCombatMode.Equals("move"))
                            {
                                currentCombatMode = "info";
                            }
                            else
                            {
                                if (isPlayerTurn)
                                {
                                    currentCombatMode = "move";
                                }
                            }                            
                            gv.screenType = "combat";
                        }
                    }
                    else if (rtn.Equals("btnInventory"))
                    {
                        if (isPlayerTurn)
                        {
                            gv.screenType = "combatInventory";
                            gv.screenInventory.resetInventory();
                        }
                    }

                    else if (rtn.Equals("btnIni1"))
                    {
                        int buttonThreshold = 1 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }
                        
                            MoveOrder m = moveOrderList[index1];
                            if (m.PcOrCreature is Player)
                            {
                                Player crt = (Player)m.PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                    UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                                }
                            }
                            if (m.PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)m.PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                    UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                                }
                            }
                        }

                    else if (rtn.Equals("btnIni2"))
                    {
                        int buttonThreshold = 2 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }

                    else if (rtn.Equals("btnIni3"))
                    {
                        int buttonThreshold = 3 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }

                    else if (rtn.Equals("btnIni4"))
                    {
                        int buttonThreshold = 4 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }

                    else if (rtn.Equals("btnIni5"))
                    {
                        int buttonThreshold = 5 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }

                    else if (rtn.Equals("btnIni6"))
                    {
                        int buttonThreshold = 6 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }

                    else if (rtn.Equals("btnIni7"))
                    {
                        int buttonThreshold = 7 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni8"))
                    {
                        int buttonThreshold = 8 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni9"))
                    {
                        int buttonThreshold = 9 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni10"))
                    {
                        int buttonThreshold = 10 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni11"))
                    {
                        int buttonThreshold = 11 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni12"))
                    {
                        int buttonThreshold = 12 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni13"))
                    {
                        int buttonThreshold = 13 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni14"))
                    {
                        int buttonThreshold = 14 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni15"))
                    {
                        int buttonThreshold = 15 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni16"))
                    {
                        int buttonThreshold = 16 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni17"))
                    {
                        int buttonThreshold = 17 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni18"))
                    {
                        int buttonThreshold = 18 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni19"))
                    {
                        int buttonThreshold = 19 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni20"))
                    {
                        int buttonThreshold = 20 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni21"))
                    {
                        int buttonThreshold = 21 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni22"))
                    {
                        int buttonThreshold = 22 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni23"))
                    {
                        int buttonThreshold = 23 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni24"))
                    {
                        int buttonThreshold = 24 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni25"))
                    {
                        int buttonThreshold = 25 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni26"))
                    {
                        int buttonThreshold = 26 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni27"))
                    {
                        int buttonThreshold = 27 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni28"))
                    {
                        int buttonThreshold = 28 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni29"))
                    {
                        int buttonThreshold = 29 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni30"))
                    {
                        int buttonThreshold = 30 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni31"))
                    {
                        int buttonThreshold = 31 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni32"))
                    {
                        int buttonThreshold = 32 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni33"))
                    {
                        int buttonThreshold = 33 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni34"))
                    {
                        int buttonThreshold = 34 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni35"))
                    {
                        int buttonThreshold = 35 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }


                    else if (rtn.Equals("btnIni36"))
                    {
                        int buttonThreshold = 36 - gv.mod.creatureCounterSubstractor;
                        int buttonCounter = 0;
                        int index1 = 0;

                        for (int i = 0; i < moveOrderList.Count; i++)
                        {
                            if (moveOrderList[i].PcOrCreature is Player)
                            {
                                Player crt = (Player)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                            if (moveOrderList[i].PcOrCreature is Creature)
                            {
                                Creature crt = (Creature)moveOrderList[i].PcOrCreature;
                                if (crt.hp > 0)
                                {
                                    buttonCounter++;
                                    if (crt.token.PixelSize.Width > 100)
                                    {
                                        buttonCounter++;
                                    }
                                    if (buttonCounter >= buttonThreshold)
                                    {
                                        index1 = i;
                                        break;
                                    }
                                }
                            }
                        }

                        MoveOrder m = moveOrderList[index1];
                        if (m.PcOrCreature is Player)
                        {
                            Player crt = (Player)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                        if (m.PcOrCreature is Creature)
                        {
                            Creature crt = (Creature)m.PcOrCreature;
                            if (crt.hp > 0)
                            {
                                UpperLeftSquare.X = crt.combatLocX - gv.playerOffsetX;
                                UpperLeftSquare.Y = crt.combatLocY - gv.playerOffsetY;
                            }
                        }
                    }

                    else if (rtn.Equals("btnAttack"))
                    {
                        if (isPlayerTurn)
                        {
                            if (currentCombatMode.Equals("attack"))
                            {
                                currentCombatMode = "info";
                            }
                            else
                            {
                                currentCombatMode = "attack";
                            }
                            gv.screenType = "combat";
                            setTargetHighlightStartLocation(pc);
                        }
                    }
                    else if (rtn.Equals("btnCast"))
                    {
                        if (isPlayerTurn)
                        {
                            if (pc.knownSpellsTags.Count > 0)
                            {
                                currentCombatMode = "castSelector";
                                gv.screenType = "combatCast";
                                gv.screenCastSelector.castingPlayerIndex = currentPlayerIndex;
                                spellSelectorIndex = 0;
                                setTargetHighlightStartLocation(pc);
                            }
                        }
                        else
                        {
                            //TODO Toast.makeText(gv.gameContext, "PC has no Spells", Toast.LENGTH_SHORT).show();
                        }
                    }
                    else if (rtn.Equals("btnSkipTurn"))
                    {
                        if (isPlayerTurn)
                        {
                            gv.screenType = "combat";
                            endPcTurn(false);
                        }
                    }
                    else if (rtn.Equals("btnSelect"))
                    {
                        if (isPlayerTurn)
                        {
                            if (currentCombatMode.Equals("attack"))
                            {
                                TargetAttackPressed(pc);
                            }
                            else if (currentCombatMode.Equals("cast"))
                            {
                                TargetCastPressed(pc);
                            }
                        }
                    }
                    else if (rtn.Equals("btnToggleArrows"))
                    {
                        foreach (IB2Panel pnl in combatUiLayout.panelList)
                        {
                            if (pnl.tag.Equals("arrowPanel"))
                            {
                                //hides down
                                showArrows = !showArrows;
                                if (pnl.currentLocY > pnl.shownLocY)
                                {
                                    pnl.showing = true;
                                }
                                else
                                {
                                    pnl.hiding = true;
                                }
                            }
                        }
                    }
                    break;
                    #endregion
            }
        }

        #endregion

        public void doUpdate(Player pc)
        {
            CalculateUpperLeft();
            if (moveCost == mod.diagonalMoveCost)
            {
                currentMoves += mod.diagonalMoveCost;
                moveCost = 1.0f;
            }
            else
            {
                currentMoves++;
            }
            float moveleft = pc.moveDistance - currentMoves;
            if (moveleft < 1) { moveleft = 0; }
        }
        public void MoveTargetHighlight(int numPadDirection)
        {
            switch (numPadDirection)
            {
                case 8: //up
                    if (targetHighlightCenterLocation.Y > 0) 
                    {
                        targetHighlightCenterLocation.Y--;
                        if (!IsInVisibleCombatWindow(targetHighlightCenterLocation.X, targetHighlightCenterLocation.Y))
                        {
                            targetHighlightCenterLocation.Y++;
                        }
                    }
                    break;
                case 2: //down
                    if (targetHighlightCenterLocation.Y < mod.currentEncounter.MapSizeY - 1)
                    {
                        targetHighlightCenterLocation.Y++;
                        if (!IsInVisibleCombatWindow(targetHighlightCenterLocation.X, targetHighlightCenterLocation.Y))
                        {
                            targetHighlightCenterLocation.Y--;
                        }
                    }
                    break;
                case 4: //left
                    if (targetHighlightCenterLocation.X > 0)
                    {
                        targetHighlightCenterLocation.X--;
                        if (!IsInVisibleCombatWindow(targetHighlightCenterLocation.X, targetHighlightCenterLocation.Y))
                        {
                            targetHighlightCenterLocation.X++;
                        }
                    }
                    break;
                case 6: //right
                    if (targetHighlightCenterLocation.X < mod.currentEncounter.MapSizeX - 1)
                    {
                        targetHighlightCenterLocation.X++;
                        if (!IsInVisibleCombatWindow(targetHighlightCenterLocation.X, targetHighlightCenterLocation.Y))
                        {
                            targetHighlightCenterLocation.X--;
                        }
                    }
                    break;
                case 9: //upright
                    if ((targetHighlightCenterLocation.X < mod.currentEncounter.MapSizeX - 1) && (targetHighlightCenterLocation.Y > 0))
                    {
                        targetHighlightCenterLocation.X++;
                        targetHighlightCenterLocation.Y--;
                        if (!IsInVisibleCombatWindow(targetHighlightCenterLocation.X, targetHighlightCenterLocation.Y))
                        {
                            targetHighlightCenterLocation.X--;
                            targetHighlightCenterLocation.Y++;
                        }
                    }
                    break;
                case 3: //downright
                    if ((targetHighlightCenterLocation.X < mod.currentEncounter.MapSizeX - 1) && (targetHighlightCenterLocation.Y < mod.currentEncounter.MapSizeY - 1))
                    {
                        targetHighlightCenterLocation.X++;
                        targetHighlightCenterLocation.Y++;
                        if (!IsInVisibleCombatWindow(targetHighlightCenterLocation.X, targetHighlightCenterLocation.Y))
                        {
                            targetHighlightCenterLocation.X--;
                            targetHighlightCenterLocation.Y--;
                        }
                    }
                    break;
                case 7: //upleft
                    if ((targetHighlightCenterLocation.X > 0) && (targetHighlightCenterLocation.Y > 0))
                    {
                        targetHighlightCenterLocation.X--;
                        targetHighlightCenterLocation.Y--;
                        if (!IsInVisibleCombatWindow(targetHighlightCenterLocation.X, targetHighlightCenterLocation.Y))
                        {
                            targetHighlightCenterLocation.X++;
                            targetHighlightCenterLocation.Y++;
                        }
                    }
                    break;
                case 1: //downleft
                    if ((targetHighlightCenterLocation.X > 0) && (targetHighlightCenterLocation.Y < mod.currentEncounter.MapSizeY - 1))
                    {
                        targetHighlightCenterLocation.X--;
                        targetHighlightCenterLocation.Y++;
                        if (!IsInVisibleCombatWindow(targetHighlightCenterLocation.X, targetHighlightCenterLocation.Y))
                        {
                            targetHighlightCenterLocation.X++;
                            targetHighlightCenterLocation.Y--;
                        }
                    }
                    break;
            }
            
        }
        public void MoveUp(Player pc)
        {
            if (pc.combatLocY > 0)
            {
                //check is walkable (blocked square or PC)
                if (isWalkable(pc.combatLocX, pc.combatLocY - 1))
                {
                    //check if creature -> do attack
                    Creature c = isBumpIntoCreature(pc.combatLocX, pc.combatLocY - 1);
                    if (c != null)
                    {
                        //attack creature
                        targetHighlightCenterLocation.X = pc.combatLocX;
                        targetHighlightCenterLocation.Y = pc.combatLocY - 1;
                        currentCombatMode = "attack";
                        TargetAttackPressed(pc);
                    }
                    else if ((pc.moveDistance - currentMoves) >= 1.0f)
                    {
                        LeaveThreatenedCheck(pc, pc.combatLocX, pc.combatLocY - 1);
                        doPlayerCombatFacing(pc, pc.combatLocX, pc.combatLocY - 1);
                        pc.combatLocY--;
                        doUpdate(pc);
                    }
                }
            }
        }
        public void MoveUpRight(Player pc)
        {
            if ((pc.combatLocX < mod.currentEncounter.MapSizeX - 1) && (pc.combatLocY > 0))
            {
                if (isWalkable(pc.combatLocX + 1, pc.combatLocY - 1))
                {
                    Creature c = isBumpIntoCreature(pc.combatLocX + 1, pc.combatLocY - 1);
                    if (c != null)
                    {

                        targetHighlightCenterLocation.X = pc.combatLocX + 1;
                        targetHighlightCenterLocation.Y = pc.combatLocY - 1;
                        currentCombatMode = "attack";
                        TargetAttackPressed(pc);
                    }
                    else if ((pc.moveDistance - currentMoves) >= mod.diagonalMoveCost)
                    {
                        LeaveThreatenedCheck(pc, pc.combatLocX + 1, pc.combatLocY - 1);
                        doPlayerCombatFacing(pc, pc.combatLocX + 1, pc.combatLocY - 1);
                        pc.combatLocX++;
                        pc.combatLocY--;
                        if (pc.combatFacingLeft)
                        {
                            pc.combatFacingLeft = false;
                        }
                        moveCost = mod.diagonalMoveCost;
                        doUpdate(pc);
                    }
                }
            }
        }
        public void MoveUpLeft(Player pc)
        {
            if ((pc.combatLocX > 0) && (pc.combatLocY > 0))
            {
                if (isWalkable(pc.combatLocX - 1, pc.combatLocY - 1))
                {
                    Creature c = isBumpIntoCreature(pc.combatLocX - 1, pc.combatLocY - 1);
                    if (c != null)
                    {

                        targetHighlightCenterLocation.X = pc.combatLocX - 1;
                        targetHighlightCenterLocation.Y = pc.combatLocY - 1;
                        currentCombatMode = "attack";
                        TargetAttackPressed(pc);
                    }
                    else if ((pc.moveDistance - currentMoves) >= mod.diagonalMoveCost)
                    {
                        LeaveThreatenedCheck(pc, pc.combatLocX - 1, pc.combatLocY - 1);
                        doPlayerCombatFacing(pc, pc.combatLocX - 1, pc.combatLocY - 1);
                        pc.combatLocX--;
                        pc.combatLocY--;
                        if (!pc.combatFacingLeft)
                        {
                            pc.combatFacingLeft = true;
                        }
                        moveCost = mod.diagonalMoveCost;
                        doUpdate(pc);
                    }
                }
            }
        }
        public void MoveDown(Player pc)
        {
            if (pc.combatLocY < mod.currentEncounter.MapSizeY - 1)
            {
                if (isWalkable(pc.combatLocX, pc.combatLocY + 1))
                {
                    Creature c = isBumpIntoCreature(pc.combatLocX, pc.combatLocY + 1);
                    if (c != null)
                    {

                        targetHighlightCenterLocation.X = pc.combatLocX;
                        targetHighlightCenterLocation.Y = pc.combatLocY + 1;
                        currentCombatMode = "attack";
                        TargetAttackPressed(pc);
                    }
                    else if ((pc.moveDistance - currentMoves) >= 1.0f)
                    {
                        LeaveThreatenedCheck(pc, pc.combatLocX, pc.combatLocY + 1);
                        doPlayerCombatFacing(pc, pc.combatLocX, pc.combatLocY + 1);
                        pc.combatLocY++;
                        doUpdate(pc);
                    }
                }
            }
        }
        public void MoveDownRight(Player pc)
        {
            if ((pc.combatLocX < mod.currentEncounter.MapSizeX - 1) && (pc.combatLocY < mod.currentEncounter.MapSizeY - 1))
            {
                if (isWalkable(pc.combatLocX + 1, pc.combatLocY + 1))
                {
                    Creature c = isBumpIntoCreature(pc.combatLocX + 1, pc.combatLocY + 1);
                    if (c != null)
                    {

                        targetHighlightCenterLocation.X = pc.combatLocX + 1;
                        targetHighlightCenterLocation.Y = pc.combatLocY + 1;
                        currentCombatMode = "attack";
                        TargetAttackPressed(pc);
                    }
                    else if ((pc.moveDistance - currentMoves) >= mod.diagonalMoveCost)
                    {
                        LeaveThreatenedCheck(pc, pc.combatLocX + 1, pc.combatLocY + 1);
                        doPlayerCombatFacing(pc, pc.combatLocX + 1, pc.combatLocY + 1);
                        pc.combatLocX++;
                        pc.combatLocY++;
                        if (pc.combatFacingLeft)
                        {
                            pc.combatFacingLeft = false;
                        }
                        moveCost = mod.diagonalMoveCost;
                        doUpdate(pc);
                    }
                }
            }
        }
        public void MoveDownLeft(Player pc)
        {
            if ((pc.combatLocX > 0) && (pc.combatLocY < mod.currentEncounter.MapSizeY - 1))
            {
                if (isWalkable(pc.combatLocX - 1, pc.combatLocY + 1))
                {
                    Creature c = isBumpIntoCreature(pc.combatLocX - 1, pc.combatLocY + 1);
                    if (c != null)
                    {

                        targetHighlightCenterLocation.X = pc.combatLocX - 1;
                        targetHighlightCenterLocation.Y = pc.combatLocY + 1;
                        currentCombatMode = "attack";
                        TargetAttackPressed(pc);
                    }
                    else if ((pc.moveDistance - currentMoves) >= mod.diagonalMoveCost)
                    {
                        LeaveThreatenedCheck(pc, pc.combatLocX - 1, pc.combatLocY + 1);
                        doPlayerCombatFacing(pc, pc.combatLocX - 1, pc.combatLocY + 1);
                        pc.combatLocX--;
                        pc.combatLocY++;
                        if (!pc.combatFacingLeft)
                        {
                            pc.combatFacingLeft = true;
                        }
                        moveCost = mod.diagonalMoveCost;
                        doUpdate(pc);
                    }
                }
            }
        }
        public void MoveRight(Player pc)
        {
            if (pc.combatLocX < mod.currentEncounter.MapSizeX - 1)
            {
                if (isWalkable(pc.combatLocX + 1, pc.combatLocY))
                {
                    Creature c = isBumpIntoCreature(pc.combatLocX + 1, pc.combatLocY);
                    if (c != null)
                    {

                        targetHighlightCenterLocation.X = pc.combatLocX + 1;
                        targetHighlightCenterLocation.Y = pc.combatLocY;
                        currentCombatMode = "attack";
                        TargetAttackPressed(pc);
                    }
                    else if ((pc.moveDistance - currentMoves) >= 1.0f)
                    {
                        LeaveThreatenedCheck(pc, pc.combatLocX + 1, pc.combatLocY);
                        doPlayerCombatFacing(pc, pc.combatLocX + 1, pc.combatLocY);
                        pc.combatLocX++;
                        if (pc.combatFacingLeft)
                        {
                            pc.combatFacingLeft = false;
                        }
                        doUpdate(pc);
                    }
                }
            }
        }
        public void MoveLeft(Player pc)
        {
            if (pc.combatLocX > 0)
            {
                if (isWalkable(pc.combatLocX - 1, pc.combatLocY))
                {
                    Creature c = isBumpIntoCreature(pc.combatLocX - 1, pc.combatLocY);
                    if (c != null)
                    {

                        targetHighlightCenterLocation.X = pc.combatLocX - 1;
                        targetHighlightCenterLocation.Y = pc.combatLocY;
                        currentCombatMode = "attack";
                        TargetAttackPressed(pc);
                    }
                    else if ((pc.moveDistance - currentMoves) >= 1.0f)
                    {
                        LeaveThreatenedCheck(pc, pc.combatLocX, pc.combatLocY);
                        doPlayerCombatFacing(pc, pc.combatLocX - 1, pc.combatLocY);
                        pc.combatLocX--;
                        if (!pc.combatFacingLeft)
                        {
                            pc.combatFacingLeft = true;
                        }
                        doUpdate(pc);
                    }                    
                }
            }
        }
        public void TargetAttackPressed(Player pc)
        {
            if (isValidAttackTarget(pc))
            {
                if ((targetHighlightCenterLocation.X < pc.combatLocX) && (!pc.combatFacingLeft)) //attack left
                {
                    pc.combatFacingLeft = true;
                }
                else if ((targetHighlightCenterLocation.X > pc.combatLocX) && (pc.combatFacingLeft)) //attack right
                {
                    pc.combatFacingLeft = false;
                }
                doPlayerCombatFacing(pc, targetHighlightCenterLocation.X, targetHighlightCenterLocation.Y);
                gv.touchEnabled = false;
                creatureToAnimate = null;
                playerToAnimate = pc;
                //set attack animation and do a delay
                attackAnimationTimeElapsed = 0;
                attackAnimationLengthInMilliseconds = (int)(5f * mod.combatAnimationSpeed);
                if ((mod.getItemByResRefForInfo(pc.MainHandRefs.resref).category.Equals("Melee"))
                        || (mod.getItemByResRefForInfo(pc.MainHandRefs.resref).name.Equals("none"))
                        || (mod.getItemByResRefForInfo(pc.AmmoRefs.resref).name.Equals("none")))
                {
                    //do melee attack stuff and animations  
                    AnimationSequence newSeq = new AnimationSequence();
                    animationSeqStack.Add(newSeq);
                    doCombatAttack(pc);
                    //add hit or miss animation
                    //add floaty text
                    //add death animations
                    AnimationStackGroup newGroup = new AnimationStackGroup();
                    animationSeqStack[0].AnimationSeq.Add(newGroup);                    
                    foreach (Coordinate coor in deathAnimationLocations)
                    {
                        if (!IsInVisibleCombatWindow(coor.X, coor.Y))
                        {
                            continue;
                        }
                        addDeathAnimation(newGroup, new Coordinate(getPixelLocX(coor.X), getPixelLocY(coor.Y)));
                    }
                    animationsOn = true;
                }
                else //Ranged Attack
                {
                    //play attack sound for ranged
                    gv.PlaySound(mod.getItemByResRefForInfo(pc.MainHandRefs.resref).itemOnUseSound);
                    //do ranged attack stuff and animations
                    //add projectile animation
                    int startX = getPixelLocX(pc.combatLocX);
                    int startY = getPixelLocY(pc.combatLocY);
                    int endX = getPixelLocX(targetHighlightCenterLocation.X);
                    int endY = getPixelLocY(targetHighlightCenterLocation.Y);
                    string filename = mod.getItemByResRefForInfo(pc.AmmoRefs.resref).projectileSpriteFilename;                    
                    AnimationSequence newSeq = new AnimationSequence();
                    animationSeqStack.Add(newSeq);
                    AnimationStackGroup newGroup = new AnimationStackGroup();
                    newSeq.AnimationSeq.Add(newGroup);
                    launchProjectile(filename, startX, startY, endX, endY, newGroup);
                    //add ending projectile animation  
                    doCombatAttack(pc);
                    //add hit or miss animation
                    //add floaty text
                    //add death animations
                    newGroup = new AnimationStackGroup();
                    animationSeqStack[0].AnimationSeq.Add(newGroup);
                    foreach (Coordinate coor in deathAnimationLocations)
                    {
                        if (!IsInVisibleCombatWindow(coor.X, coor.Y))
                        {
                            continue;
                        }
                        addDeathAnimation(newGroup, new Coordinate(getPixelLocX(coor.X), getPixelLocY(coor.Y)));
                    }
                    animationsOn = true;
                }
            }
        }
        public void TargetCastPressed(Player pc)
        {
            //Uses Map Pixel Locations
            int endX = targetHighlightCenterLocation.X * gv.squareSize + (gv.squareSize / 2);
            int endY = targetHighlightCenterLocation.Y * gv.squareSize + (gv.squareSize / 2);
            int startX = pc.combatLocX * gv.squareSize + (gv.squareSize / 2);
            int startY = pc.combatLocY * gv.squareSize + (gv.squareSize / 2);

            if ((isValidCastTarget(pc)) && (isVisibleLineOfSight(new Coordinate(endX, endY), new Coordinate(startX, startY))))
            {
                if ((targetHighlightCenterLocation.X < pc.combatLocX) && (!pc.combatFacingLeft)) //attack left
                {
                    pc.combatFacingLeft = true;
                }
                else if ((targetHighlightCenterLocation.X > pc.combatLocX) && (pc.combatFacingLeft)) //attack right
                {
                    pc.combatFacingLeft = false;
                }
                doPlayerCombatFacing(pc, targetHighlightCenterLocation.X, targetHighlightCenterLocation.Y);
                gv.touchEnabled = false;
                creatureToAnimate = null;
                playerToAnimate = pc;
                //set attack animation and do a delay
                attackAnimationTimeElapsed = 0;
                attackAnimationLengthInMilliseconds = (int)(5f * mod.combatAnimationSpeed);
                AnimationSequence newSeq = new AnimationSequence();
                animationSeqStack.Add(newSeq);
                //add projectile animation
                gv.PlaySound(gv.cc.currentSelectedSpell.spellStartSound);
                startX = getPixelLocX(pc.combatLocX);
                startY = getPixelLocY(pc.combatLocY);
                endX = getPixelLocX(targetHighlightCenterLocation.X);
                endY = getPixelLocY(targetHighlightCenterLocation.Y);
                string filename = gv.cc.currentSelectedSpell.spriteFilename;
                AnimationStackGroup newGroup = new AnimationStackGroup();
                newSeq.AnimationSeq.Add(newGroup);
                launchProjectile(filename, startX, startY, endX, endY, newGroup);
                //gv.PlaySound(gv.cc.currentSelectedSpell.spellEndSound);
                object target = getCastTarget(pc);
                gv.cc.doSpellBasedOnScriptOrEffectTag(gv.cc.currentSelectedSpell, pc, target);
                //add ending projectile animation
                newGroup = new AnimationStackGroup();
                animationSeqStack[0].AnimationSeq.Add(newGroup);
                filename = gv.cc.currentSelectedSpell.spriteEndingFilename;
                foreach (Coordinate coor in gv.sf.AoeSquaresList)
                {
                    if (!IsInVisibleCombatWindow(coor.X, coor.Y))
                    {
                        continue;
                    }
                    addEndingAnimation(newGroup, new Coordinate(getPixelLocX(coor.X), getPixelLocY(coor.Y)), filename);
                }
                //add floaty text
                //add death animations
                newGroup = new AnimationStackGroup();
                animationSeqStack[0].AnimationSeq.Add(newGroup);
                foreach (Coordinate coor in deathAnimationLocations)
                {
                    if (!IsInVisibleCombatWindow(coor.X, coor.Y))
                    {
                        continue;
                    }
                    addDeathAnimation(newGroup, new Coordinate(getPixelLocX(coor.X), getPixelLocY(coor.Y)));
                }
                animationsOn = true;
            }
        }
        public void launchProjectile(string filename, int startX, int startY, int endX, int endY, AnimationStackGroup group)
        {            
            //calculate angle from start to end point
            float angle = AngleRad(new Point(startX, startY), new Point(endX, endY));
            float dX = (endX - startX);
            float dY = (endY - startY);
            //calculate needed TimeToLive based on a constant speed for projectiles
            int ttl = 1000;
            float speed = 2f; //small number is faster travel speed
            if (Math.Abs(dX) > Math.Abs(dY))
            {
                ttl = (int)(Math.Abs(dX) * speed);
            }
            else
            {
                ttl = (int)(Math.Abs(dY) * speed);
            }
            SharpDX.Vector2 vel = SharpDX.Vector2.Normalize(new SharpDX.Vector2(dX, dY));
            Sprite spr = new Sprite(gv, filename, startX, startY, vel.X / speed, vel.Y / speed, angle, 0, 1.0f, ttl, false, 100);
            group.SpriteGroup.Add(spr);
        }
        public void addHitAnimation(AnimationStackGroup group)
        {
            if (gv.mod.useManualCombatCam)
            {
                gv.touchEnabled = false;
            }
            int ttl = 8 * mod.combatAnimationSpeed;
            Sprite spr = new Sprite(gv, "hit_symbol", hitAnimationLocation.X, hitAnimationLocation.Y, 0, 0, 0, 0, 1.0f, ttl, false, ttl / 4);
            group.turnFloatyTextOn = true;
            group.SpriteGroup.Add(spr);
        }
        public void addMissAnimation(AnimationStackGroup group)
        {
            if (gv.mod.useManualCombatCam)
            {
                gv.touchEnabled = false;
            }
            int ttl = 8 * mod.combatAnimationSpeed;
            Sprite spr = new Sprite(gv, "miss_symbol", hitAnimationLocation.X, hitAnimationLocation.Y, 0, 0, 0, 0, 1.0f, ttl, false, ttl / 4);
            group.SpriteGroup.Add(spr);
        }
        public void addDeathAnimation(AnimationStackGroup group, Coordinate Loc)
        {
            if (gv.mod.useManualCombatCam)
            {
                gv.touchEnabled = false;
            }
            int ttl = 16 * mod.combatAnimationSpeed;
            Sprite spr = new Sprite(gv, "death_fx", Loc.X, Loc.Y, 0, 0, 0, 0, 1.0f, ttl, false, ttl / 4);
            group.SpriteGroup.Add(spr);
        }
        public void addEndingAnimation(AnimationStackGroup group, Coordinate Loc, string filename)
        {
            if (gv.mod.useManualCombatCam)
            {
                gv.touchEnabled = false;
            }

            int ttl = 16 * mod.combatAnimationSpeed;
            if (gv.mod.useManualCombatCam)
            {
                ttl = 8 * mod.combatAnimationSpeed;
            }
            
            Sprite spr = new Sprite(gv, filename, Loc.X, Loc.Y, 0, 0, 0, 0, 1.0f, ttl, false, ttl / 4);
            group.turnFloatyTextOn = true;
            group.SpriteGroup.Add(spr);
        }
        public float AngleRad(Point start, Point end)
        {
            return (float)(-1 * ((Math.Atan2(start.Y - end.Y, end.X - start.X)) - (Math.PI) / 2));
        }

        //Helper Methods
        public void CalculateUpperLeft()
        {
            Player pc = mod.playerList[currentPlayerIndex];
            int minX = pc.combatLocX - gv.playerOffsetX;
            if (minX < 0) { minX = 0; }
            int minY = pc.combatLocY - gv.playerOffsetY;
            if (minY < 0) { minY = 0; }
                        
            if ((pc.combatLocX <= (UpperLeftSquare.X + 7)) && (pc.combatLocX >= UpperLeftSquare.X + 2) && (pc.combatLocY <= (UpperLeftSquare.Y + 7)) && (pc.combatLocY >= UpperLeftSquare.Y + 2))
            { 
                return; 
            }
            else
            { 
                UpperLeftSquare.X = minX; 
                UpperLeftSquare.Y = minY; 
            }
        }
        public void CalculateUpperLeftCreature()
        {
            Creature crt = mod.currentEncounter.encounterCreatureList[creatureIndex];
            int minX = crt.combatLocX - gv.playerOffsetX;
            if (minX < 0) { minX = 0; }
            int minY = crt.combatLocY - gv.playerOffsetY;
            if (minY < 0) { minY = 0; }

            //do not adjust view port if creature is on screen already and ends move at least one square away from border
            if (((crt.combatLocX + 2) <= (UpperLeftSquare.X + (gv.playerOffsetX * 2))) && ((crt.combatLocX - 2) >= (UpperLeftSquare.X)) && ((crt.combatLocY + 2) <= (UpperLeftSquare.Y + (gv.playerOffsetY * 2))) && ((crt.combatLocY - 2) >= (UpperLeftSquare.Y)))
            {
                return;
            }

            else
            {
                if (gv.mod.useManualCombatCam)
                {
                    //Melee or AoO situation
                    foreach (Player p in mod.playerList)
                    {
                        if (getDistance(new Coordinate(p.combatLocX, p.combatLocY), new Coordinate(crt.combatLocX, crt.combatLocY)) <= 1)
                        {
                            UpperLeftSquare.X = minX;
                            UpperLeftSquare.Y = minY;
                            break;
                        }
                        if (adjustCamToRangedCreature)
                        {
                            if (getDistance(new Coordinate(p.combatLocX, p.combatLocY), new Coordinate(crt.combatLocX, crt.combatLocY)) < 9)
                            {
                                if (p.combatLocX < crt.combatLocX)
                                {
                                    UpperLeftSquare.X = p.combatLocX;
                                }
                                else
                                {
                                    UpperLeftSquare.X = crt.combatLocX;
                                }

                                if (p.combatLocY < crt.combatLocY)
                                {
                                    UpperLeftSquare.Y = p.combatLocY;
                                }
                                else
                                {
                                    UpperLeftSquare.Y = crt.combatLocY;
                                }
                                break;
                            }
                         }
                    }

                    //return;
                }
                else
                {
                    UpperLeftSquare.X = minX;
                    UpperLeftSquare.Y = minY;
                }
            }
        }
        public void CenterScreenOnPC()
        {
            Player pc = mod.playerList[currentPlayerIndex];
            int minX = pc.combatLocX - gv.playerOffsetX;
            if (minX < 0) { minX = 0; }
            int minY = pc.combatLocY - gv.playerOffsetY;
            if (minY < 0) { minY = 0; }

            UpperLeftSquare.X = minX;
            UpperLeftSquare.Y = minY; 
        }
        public bool IsInVisibleCombatWindow(int sqrX, int sqrY)
        {
            //all input coordinates are in Map Location, not Screen Location
            if ((sqrX < UpperLeftSquare.X) || (sqrY < UpperLeftSquare.Y))
            {
                return false;
            }

            if ((sqrX < 0) || (sqrY < 0))
            {
                return false;
            }

            if ((sqrX >= UpperLeftSquare.X + gv.playerOffsetX + gv.playerOffsetX + 1)
                || (sqrY >= UpperLeftSquare.Y + gv.playerOffsetY + gv.playerOffsetY + 2))
            {
                return false;
            }

            if ((sqrX >= gv.mod.currentEncounter.MapSizeX)
                || (sqrY >= gv.mod.currentEncounter.MapSizeY))
            {
                return false;
            }
            return true;
        }
        public bool IsInVisibleCombatWindow(int sqrX, int sqrY, int tileW, int tileH)
        {
            //all input coordinates are in Map Location, not Screen Location
            if ((sqrX < UpperLeftSquare.X) || (sqrY < UpperLeftSquare.Y))
            {
                return false;
            }
            if ((sqrX >= UpperLeftSquare.X + gv.playerOffsetX + gv.playerOffsetX + 1)
                || (sqrY >= UpperLeftSquare.Y + gv.playerOffsetY + gv.playerOffsetY + 2))
            {
                return false;
            }
            return true;
        }
        public int getPixelLocX(int sqrX)
        {
            return ((sqrX - UpperLeftSquare.X) * gv.squareSize) + gv.oXshift + mapStartLocXinPixels;
        }
        public int getPixelLocY(int sqrY)
        {
            return (sqrY - UpperLeftSquare.Y) * gv.squareSize;
        }
        public int getViewportSquareLocX(int sqrX)
        {
            return sqrX - UpperLeftSquare.X;
        }
        public int getViewportSquareLocY(int sqrY)
        {
            return sqrY - UpperLeftSquare.Y;
        }
	    public void setTargetHighlightStartLocation(Player pc)
	    {
		    targetHighlightCenterLocation.X = pc.combatLocX;
		    targetHighlightCenterLocation.Y = pc.combatLocY;		
	    }
	    public bool isValidAttackTarget(Player pc)
	    {
		    if (isInRange(pc))
		    {
                Item it = mod.getItemByResRefForInfo(pc.MainHandRefs.resref);
                //if using ranged and have ammo, use ammo properties
                if ((mod.getItemByResRefForInfo(pc.MainHandRefs.resref).category.Equals("Ranged"))
                        && (!mod.getItemByResRefForInfo(pc.AmmoRefs.resref).name.Equals("none")))
                {
                    //ranged weapon with ammo
                    it = mod.getItemByResRefForInfo(pc.AmmoRefs.resref);
                }
                if (it == null)
                {
                    it = mod.getItemByResRefForInfo(pc.MainHandRefs.resref);
                }
                //check to see if is AoE or Point Target else needs a target PC or Creature
                if (it.AreaOfEffect > 0)
                {
                    return true;
                }

                //Uses the Map Pixel Locations
                int endX2 = targetHighlightCenterLocation.X * gv.squareSize + (gv.squareSize / 2);
                int endY2 = targetHighlightCenterLocation.Y * gv.squareSize + (gv.squareSize / 2);
                int startX2 = pc.combatLocX * gv.squareSize + (gv.squareSize / 2);
                int startY2 = pc.combatLocY * gv.squareSize + (gv.squareSize / 2);

			    if ((isVisibleLineOfSight(new Coordinate(endX2, endY2), new Coordinate(startX2, startY2)))
				    || (getDistance(new Coordinate(pc.combatLocX,pc.combatLocY), targetHighlightCenterLocation) == 1))
			    {
				    foreach (Creature crt in mod.currentEncounter.encounterCreatureList)
				    {
					    if ((crt.combatLocX == targetHighlightCenterLocation.X) && (crt.combatLocY == targetHighlightCenterLocation.Y))
					    {
						    return true;
					    }
				    }
			    }
		    }
		    return false;
	    }
	    public bool isValidCastTarget(Player pc)
	    {
		    if (isInRange(pc))
		    {
			    //check to see if is AoE or Point Target else needs a target PC or Creature
			    if ((gv.cc.currentSelectedSpell.aoeRadius > 0) || (gv.cc.currentSelectedSpell.spellTargetType.Equals("PointLocation")))
			    {
				    return true;
			    }			
			    //is not an AoE ranged attack, is a PC or Creature
			    else
			    {
				    //check to see if target is a friend or self
				    if ((gv.cc.currentSelectedSpell.spellTargetType.Equals("Friend")) || (gv.cc.currentSelectedSpell.spellTargetType.Equals("Self")))
				    {
					    foreach (Player p in mod.playerList)
					    {
						    if ((p.combatLocX == targetHighlightCenterLocation.X) && (p.combatLocY == targetHighlightCenterLocation.Y))
						    {
							    return true;
						    }
					    }
				    }
				    else //target is a creature
				    {
					    foreach (Creature crt in mod.currentEncounter.encounterCreatureList)
					    {
						    if ((crt.combatLocX == targetHighlightCenterLocation.X) && (crt.combatLocY == targetHighlightCenterLocation.Y))
						    {
							    return true;
						    }
					    }
				    }
			    }			
		    }
		    return false;
	    }
	    public object getCastTarget(Player pc)
	    {
		    if (isInRange(pc))
		    {
			    //check to see if is AoE or Point Target else needs a target PC or Creature
			    if ((gv.cc.currentSelectedSpell.aoeRadius > 0) || (gv.cc.currentSelectedSpell.spellTargetType.Equals("PointLocation")))
			    {
				    return new Coordinate(targetHighlightCenterLocation.X, targetHighlightCenterLocation.Y);
			    }			
			    //is not an AoE ranged attack, is a PC or Creature
			    else
			    {
				    //check to see if target is a friend or self
				    if ((gv.cc.currentSelectedSpell.spellTargetType.Equals("Friend")) || (gv.cc.currentSelectedSpell.spellTargetType.Equals("Self")))
				    {
					    foreach (Player p in mod.playerList)
					    {
						    if ((p.combatLocX == targetHighlightCenterLocation.X) && (p.combatLocY == targetHighlightCenterLocation.Y))
						    {
							    return p;
						    }
					    }
				    }
				    else //target is a creature
				    {
					    foreach (Creature crt in mod.currentEncounter.encounterCreatureList)
					    {
						    if ((crt.combatLocX == targetHighlightCenterLocation.X) && (crt.combatLocY == targetHighlightCenterLocation.Y))
						    {
							    return crt;
						    }
					    }
				    }
			    }			
		    }
		    return null;
	    }
	    public bool isInRange(Player pc)
	    {
		    if (currentCombatMode.Equals("attack"))
		    {
			    int range = 1;
			    if ((mod.getItemByResRefForInfo(pc.MainHandRefs.resref).category.Equals("Ranged")) 
	        		    && (mod.getItemByResRefForInfo(pc.AmmoRefs.resref).name.Equals("none")))
	            {
				    //ranged weapon with no ammo
				    range = 1;
	            }
			    else 
			    {
				    range = mod.getItemByResRefForInfo(pc.MainHandRefs.resref).attackRange;
			    }
			
			    if (getDistance(new Coordinate(pc.combatLocX,pc.combatLocY), targetHighlightCenterLocation) <= range)
			    {
				    return true;
			    }
		    }
		    else if (currentCombatMode.Equals("cast"))
		    {
			    if (getDistance(new Coordinate(pc.combatLocX,pc.combatLocY), targetHighlightCenterLocation) <= gv.cc.currentSelectedSpell.range)
			    {
				    return true;
			    }
		    }
		    return false;
	    }
	    public bool isAdjacentEnemy(Player pc)
	    {
		    foreach (Creature crt in mod.currentEncounter.encounterCreatureList)
		    {
			    if (getDistance(new Coordinate(pc.combatLocX,pc.combatLocY), new Coordinate(crt.combatLocX,crt.combatLocY)) == 1)
			    {
				    if (!crt.isHeld())
				    {
					    return true;
				    }
			    }
		    }
		    return false;
	    }
	    public bool isAdjacentPc(Creature crt)
	    {
		    foreach (Player pc in mod.playerList)
		    {
			    if (getDistance(new Coordinate(pc.combatLocX,pc.combatLocY), new Coordinate(crt.combatLocX,crt.combatLocY)) == 1)
			    {
				    return true;
			    }
		    }
		    return false;
	    }
	    public int getGridX(Coordinate nextPoint)
        {
            int gridx = ((nextPoint.X - mapStartLocXinPixels - gv.oXshift) / gv.squareSize) + UpperLeftSquare.X;
            if (gridx > mod.currentEncounter.MapSizeX - 1) { gridx = mod.currentEncounter.MapSizeX - 1; }
            if (gridx < 0) { gridx = 0; }
            return gridx;
        }
        public int getGridY(Coordinate nextPoint)
        {
            int gridy = ((nextPoint.Y - gv.oYshift) / gv.squareSize) + UpperLeftSquare.Y;
            if (gridy > mod.currentEncounter.MapSizeY - 1) { gridy = mod.currentEncounter.MapSizeY - 1; }
            if (gridy < 0) { gridy = 0; }
            return gridy;
        }
        public int getMapSquareX(Coordinate nextPoint)
        {
            int gridx = (nextPoint.X / gv.squareSize);
            if (gridx > mod.currentEncounter.MapSizeX - 1) { gridx = mod.currentEncounter.MapSizeX - 1; }
            if (gridx < 0) { gridx = 0; }
            return gridx;
        }
        public int getMapSquareY(Coordinate nextPoint)
        {
            int gridy = (nextPoint.Y / gv.squareSize);
            if (gridy > mod.currentEncounter.MapSizeY - 1) { gridy = mod.currentEncounter.MapSizeY - 1; }
            if (gridy < 0) { gridy = 0; }
            return gridy;
        }
        public bool isVisibleLineOfSight(Coordinate end, Coordinate start)
        {  
            //This Method Uses Map Pixel Locations Only

            int deltax = Math.Abs(end.X - start.X);
            int deltay = Math.Abs(end.Y - start.Y);
            int ystep = gv.squareSize / 50;
            int xstep = gv.squareSize / 50;
            if (ystep < 1) {ystep = 1;}
            if (xstep < 1) {xstep = 1;}

            if (deltax > deltay) //Low Angle line
            {
        	    Coordinate nextPoint = start;
                int error = deltax / 2;

                if (end.Y < start.Y) { ystep = -1 * ystep; } //down and right or left

                if (end.X > start.X) //down and right
                {
                    for (int x = start.X; x <= end.X; x += xstep)
                    {
                        nextPoint.X = x;
                        error -= deltay;
                        if (error < 0)
                        {
                            nextPoint.Y += ystep;
                            error += deltax;
                        }
                        //do your checks here for LoS blocking
                        int gridx = getMapSquareX(nextPoint);
                        int gridy = getMapSquareY(nextPoint);
                        if (mod.currentEncounter.encounterTiles[gridy * mod.currentEncounter.MapSizeX + gridx].LoSBlocked)
                        {
                    	    return false;
                        }                       
                    }
                }
                else //down and left
                {
                    for (int x = start.X; x >= end.X; x -= xstep)
                    {
                        nextPoint.X = x;
                        error -= deltay;
                        if (error < 0)
                        {
                            nextPoint.Y += ystep;
                            error += deltax;
                        }
                        //do your checks here for LoS blocking
                        int gridx = getMapSquareX(nextPoint);
                        int gridy = getMapSquareY(nextPoint);
                        if (mod.currentEncounter.encounterTiles[gridy * mod.currentEncounter.MapSizeX + gridx].LoSBlocked)
                        {
                    	    return false;
                        }                      
                    }
                }
            }

            else //Low Angle line
            {
        	    Coordinate nextPoint = start;
                int error = deltay / 2;

                if (end.X < start.X) { xstep = -1 * xstep; } //up and right or left

                if (end.Y > start.Y) //up and right
                {
                    for (int y = start.Y; y <= end.Y; y += ystep)
                    {
                        nextPoint.Y = y;
                        error -= deltax;
                        if (error < 0)
                        {
                            nextPoint.X += xstep;
                            error += deltay;
                        }
                        //do your checks here for LoS blocking
                        int gridx = getMapSquareX(nextPoint);
                        int gridy = getMapSquareY(nextPoint);
                        if (mod.currentEncounter.encounterTiles[gridy * mod.currentEncounter.MapSizeX + gridx].LoSBlocked)
                        {
                    	    return false;
                        }                      
                    }
                }
                else //up and right
                {
                    for (int y = start.Y; y >= end.Y; y -= ystep)
                    {
                        nextPoint.Y = y;
                        error -= deltax;
                        if (error < 0)
                        {
                            nextPoint.X += xstep;
                            error += deltay;
                        }
                        //do your checks here for LoS blocking
                        int gridx = getMapSquareX(nextPoint);
                        int gridy = getMapSquareY(nextPoint);
                        if (mod.currentEncounter.encounterTiles[gridy * mod.currentEncounter.MapSizeX + gridx].LoSBlocked)
                        {
                    	    return false;
                        }                       
                    }
                }
            }
        
            return true;
        }
	    public bool drawVisibleLineOfSightTrail(Coordinate end, Coordinate start, Color penColor, int penWidth)
        {
            // Bresenham Line algorithm
            // Creates a line from Begin to End starting at (x0,y0) and ending at (x1,y1)
            // where x0 less than x1 and y0 less than y1
            // AND line is less steep than it is wide (dx less than dy)    
        
            int deltax = Math.Abs(end.X - start.X);
            int deltay = Math.Abs(end.Y - start.Y);
            int ystep = gv.squareSize / 50;
            int xstep = gv.squareSize / 50;
            if (ystep < 1) {ystep = 1;}
            if (xstep < 1) {xstep = 1;}

            if (deltax > deltay) //Low Angle line
            {
        	    Coordinate nextPoint = start;
                int error = deltax / 2;

                if (end.Y < start.Y) { ystep = -1 * ystep; } //down and right or left

                if (end.X > start.X) //down and right
                {
            	    int lastX = start.X;
            	    int lastY = start.Y;
                    for (int x = start.X; x <= end.X; x += xstep)
                    {
                        nextPoint.X = x;
                        error -= deltay;
                        if (error < 0)
                        {
                            nextPoint.Y += ystep;
                            error += deltax;
                        }
                        //do your checks here for LoS blocking
                        int gridx = getGridX(nextPoint);
                        int gridy = getGridY(nextPoint);
                        gv.DrawLine(lastX + gv.oXshift, lastY, nextPoint.X + gv.oXshift, nextPoint.Y, penColor, penWidth);
                        if (mod.currentEncounter.encounterTiles[gridy * mod.currentEncounter.MapSizeX + gridx].LoSBlocked)
                        {
                    	    return false;
                        }
                        lastX = nextPoint.X;
                        lastY = nextPoint.Y;
                    }
                }
                else //down and left
                {
            	    int lastX = start.X;
            	    int lastY = start.Y;                
                    for (int x = start.X; x >= end.X; x -= xstep)
                    {
                        nextPoint.X = x;
                        error -= deltay;
                        if (error < 0)
                        {
                            nextPoint.Y += ystep;
                            error += deltax;
                        }
                        //do your checks here for LoS blocking
                        int gridx = getGridX(nextPoint);
                        int gridy = getGridY(nextPoint);
                        gv.DrawLine(lastX + gv.oXshift, lastY, nextPoint.X + gv.oXshift, nextPoint.Y, penColor, penWidth);
                        if (mod.currentEncounter.encounterTiles[gridy * mod.currentEncounter.MapSizeX + gridx].LoSBlocked)
                        {
                    	    return false;
                        }
                        lastX = nextPoint.X;
                        lastY = nextPoint.Y;
                    }
                }
            }

            else //Low Angle line
            {
        	    Coordinate nextPoint = start;
                int error = deltay / 2;

                if (end.X < start.X) { xstep = -1 * xstep; } //up and right or left

                if (end.Y > start.Y) //up and right
                {
            	    int lastX = start.X;
            	    int lastY = start.Y;                
                    for (int y = start.Y; y <= end.Y; y += ystep)
                    {
                        nextPoint.Y = y;
                        error -= deltax;
                        if (error < 0)
                        {
                            nextPoint.X += xstep;
                            error += deltay;
                        }
                        //do your checks here for LoS blocking
                        int gridx = getGridX(nextPoint);
                        int gridy = getGridY(nextPoint);
                        gv.DrawLine(lastX + gv.oXshift, lastY, nextPoint.X + gv.oXshift, nextPoint.Y, penColor, penWidth);
                        if (mod.currentEncounter.encounterTiles[gridy * mod.currentEncounter.MapSizeX + gridx].LoSBlocked)
                        {
                    	    return false;
                        }
                        lastX = nextPoint.X;
                        lastY = nextPoint.Y;
                    }
                }
                else //up and right
                {
            	    int lastX = start.X;
            	    int lastY = start.Y;                
                    for (int y = start.Y; y >= end.Y; y -= ystep)
                    {
                        nextPoint.Y = y;
                        error -= deltax;
                        if (error < 0)
                        {
                            nextPoint.X += xstep;
                            error += deltay;
                        }
                        //do your checks here for LoS blocking
                        int gridx = getGridX(nextPoint);
                        int gridy = getGridY(nextPoint);
                        gv.DrawLine(lastX + gv.oXshift, lastY, nextPoint.X + gv.oXshift, nextPoint.Y, penColor, penWidth);
                        if (mod.currentEncounter.encounterTiles[gridy * mod.currentEncounter.MapSizeX + gridx].LoSBlocked)
                        {
                    	    return false;
                        }   
                        lastX = nextPoint.X;
                        lastY = nextPoint.Y;
                    }
                }
            }
        
            return true;
        }
        public bool IsAttackFromBehind(Player pc, Creature crt)
        {
            if ((pc.combatLocX >  crt.combatLocX) && (pc.combatLocY >  crt.combatLocY) && (crt.combatFacing == 7)) { return true; }
            if ((pc.combatLocX == crt.combatLocX) && (pc.combatLocY >  crt.combatLocY) && (crt.combatFacing == 8)) { return true; }
            if ((pc.combatLocX <  crt.combatLocX) && (pc.combatLocY >  crt.combatLocY) && (crt.combatFacing == 9)) { return true; }
            if ((pc.combatLocX >  crt.combatLocX) && (pc.combatLocY == crt.combatLocY) && (crt.combatFacing == 4)) { return true; }
            if ((pc.combatLocX <  crt.combatLocX) && (pc.combatLocY == crt.combatLocY) && (crt.combatFacing == 6)) { return true; }
            if ((pc.combatLocX >  crt.combatLocX) && (pc.combatLocY <  crt.combatLocY) && (crt.combatFacing == 1)) { return true; }
            if ((pc.combatLocX == crt.combatLocX) && (pc.combatLocY <  crt.combatLocY) && (crt.combatFacing == 2)) { return true; }
            if ((pc.combatLocX <  crt.combatLocX) && (pc.combatLocY <  crt.combatLocY) && (crt.combatFacing == 3)) { return true; }
            return false;
        }
        public bool IsCreatureAttackFromBehind(Player pc, Creature crt)
        {
            if ((crt.combatLocX > pc.combatLocX) && (crt.combatLocY > pc.combatLocY) && (pc.combatFacing == 7)) { return true; }
            if ((crt.combatLocX == pc.combatLocX) && (crt.combatLocY > pc.combatLocY) && (pc.combatFacing == 8)) { return true; }
            if ((crt.combatLocX < pc.combatLocX) && (crt.combatLocY > pc.combatLocY) && (pc.combatFacing == 9)) { return true; }
            if ((crt.combatLocX > pc.combatLocX) && (crt.combatLocY == pc.combatLocY) && (pc.combatFacing == 4)) { return true; }
            if ((crt.combatLocX < pc.combatLocX) && (crt.combatLocY == pc.combatLocY) && (pc.combatFacing == 6)) { return true; }
            if ((crt.combatLocX > pc.combatLocX) && (crt.combatLocY < pc.combatLocY) && (pc.combatFacing == 1)) { return true; }
            if ((crt.combatLocX == pc.combatLocX) && (crt.combatLocY < pc.combatLocY) && (pc.combatFacing == 2)) { return true; }
            if ((crt.combatLocX < pc.combatLocX) && (crt.combatLocY < pc.combatLocY) && (pc.combatFacing == 3)) { return true; }
            return false;
        }
	    public int getDistance(Coordinate start, Coordinate end)
	    {
		    int dist = 0;
            int deltaX = (int)Math.Abs((start.X - end.X));
            int deltaY = (int)Math.Abs((start.Y - end.Y));
            if (deltaX > deltaY)
                dist = deltaX;
            else
                dist = deltaY;
            return dist;
	    }
	    public bool isWalkable(int x, int y)
	    {
            if (!mod.currentEncounter.encounterTiles[y * mod.currentEncounter.MapSizeX + x].Walkable)
            {
        	    return false;
            }
		    foreach (Player p in mod.playerList)
		    {
			    if ((p.combatLocX == x) && (p.combatLocY == y))
			    {
				    if ((!p.isDead()) && (p.hp > 0))
				    {
					    return false;
				    }
			    }
		    }
		    return true;			
	    }
        public Creature isBumpIntoCreature(int x, int y)
        {
            foreach (Creature crt in mod.currentEncounter.encounterCreatureList)
            {
                if ((crt.combatLocX == x) && (crt.combatLocY == y))
                {
            	    return crt;
                }
            }
            return null;
        }
        public void LeaveThreatenedCheck(Player pc, int futurePlayerLocationX, int futurePlayerLocationY)
        {
       
            foreach (Creature crt in mod.currentEncounter.encounterCreatureList)
            {
                if ((crt.hp > 0) && (!crt.isHeld()))
                {
                    //if started in distance = 1 and now distance = 2 then do attackOfOpportunity
                    //also do attackOfOpportunity if moving within controlled area around a creature, i.e. when distance to cerature after move is still one square
                    //the later makes it harder to circle around a cretaure or break through lines, fighters get more area control this way, allwoing them to protect other charcters with more ease
                    if ( ( (CalcDistance(crt.combatLocX, crt.combatLocY, pc.combatLocX, pc.combatLocY) == 1)
                        && (CalcDistance(crt.combatLocX, crt.combatLocY, futurePlayerLocationX, futurePlayerLocationY) == 2) ) 
                        || ( (currentMoves > 0) && (CalcDistance(crt.combatLocX, crt.combatLocY, pc.combatLocX, pc.combatLocY) == 1)
                        && (CalcDistance(crt.combatLocX, crt.combatLocY, futurePlayerLocationX, futurePlayerLocationY) == 1) ) )
                    {
                        if (pc.steathModeOn)
                        {
                            gv.cc.addLogText("<font color='lime'>Avoids Attack of Opportunity due to Stealth</font><BR>");
                        }
                        else
                        {
                            gv.cc.addLogText("<font color='blue'>Attack of Opportunity by: " + crt.cr_name + "</font><BR>");
                            doActualCreatureAttack(pc, crt, 1);
                            if (pc.hp <= 0)
                            {
                                currentMoves = 99;
                            }
                        }
                    }
                }
            }
        }
	            		
	    public int CalcPcAttackModifier(Player pc, Creature crt)
        {
            int modifier = 0;
            if ((mod.getItemByResRefForInfo(pc.MainHandRefs.resref).category.Equals("Melee")) 
        		    || (mod.getItemByResRefForInfo(pc.MainHandRefs.resref).name.Equals("none"))
        		    || (mod.getItemByResRefForInfo(pc.AmmoRefs.resref).name.Equals("none")))
            {
                modifier = (pc.strength - 10) / 2;
                //if has critical strike trait use dexterity for attack modifier in melee if greater than strength modifier
                if (pc.knownTraitsTags.Contains("criticalstrike"))
			    {
            	    int modifierDex = (pc.dexterity - 10) / 2;
            	    if (modifierDex > modifier)
            	    {
            		    modifier = (pc.dexterity - 10) / 2;
            	    }
			    }
                //if doing sneak attack, bonus to hit roll
                if (pc.steathModeOn)
                {
    	            if (pc.knownTraitsTags.Contains("sneakattack"))
    			    {
    	        	    //+1 for every 2 levels after level 1
    	        	    int adding = ((pc.classLevel - 1) / 2) + 1;
    	        	    modifier += adding;
    	        	    gv.cc.addLogText("<font color='lime'> sneak attack: +" + adding + " to hit</font><BR>");
    			    }		        
                }
                //all attacks of the PC from behind get a +2 bonus to hit            
                if (IsAttackFromBehind(pc, crt))
                {
                    modifier += mod.attackFromBehindToHitModifier;
                    if (mod.attackFromBehindToHitModifier > 0)
                    {
                        gv.cc.addLogText("<font color='lime'> Attack from behind: +" + mod.attackFromBehindToHitModifier.ToString() + " to hit." + "</font><BR>");
                    }
                }
            }
            else //ranged weapon used
            {
                modifier = (pc.dexterity - 10) / 2;
                //factor in penalty for adjacent enemies when using ranged weapon
                if (isAdjacentEnemy(pc))
                {
            	    if (gv.sf.hasTrait(pc, "pointblankshot"))
            	    {
            		    //has point blank shot trait, no penalty
            	    }
            	    else
            	    {
	            	    modifier -= 4;
	            	    gv.cc.addLogText("<font color='yellow'>" + "-4 ranged attack penalty" + "</font><BR>");
	            	    gv.cc.addLogText("<font color='yellow'>" + "with enemies in melee range" + "</font><BR>");
	            	    gv.cc.addFloatyText(new Coordinate(pc.combatLocX, pc.combatLocY), "-4 att", "yellow");
            	    }
                }
                if (gv.sf.hasTrait(pc, "preciseshot2"))
        	    {
        		    modifier += 2;
        		    gv.cc.addLogText("<font color='lime'> PreciseShotL2: +2 to hit</font><BR>");
        	    }
                else if (gv.sf.hasTrait(pc, "preciseshot"))
        	    {
            	    modifier++;
            	    gv.cc.addLogText("<font color='lime'> PreciseShotL1: +1 to hit</font><BR>");
        	    }
            }
            if (gv.sf.hasTrait(pc, "hardtokill"))
            {
                modifier -= 2;
                gv.cc.addLogText("<font color='yellow'>" + "blinded by rage" + "</font><BR>");
                gv.cc.addLogText("<font color='yellow'>" + "-2 attack penalty" + "</font><BR>");
            }
            int attackMod = modifier + pc.baseAttBonus + mod.getItemByResRefForInfo(pc.MainHandRefs.resref).attackBonus;        
            Item it = mod.getItemByResRefForInfo(pc.AmmoRefs.resref);
            if (it != null)
            {
        	    attackMod += mod.getItemByResRefForInfo(pc.AmmoRefs.resref).attackBonus;
            }
            return attackMod;
        }
	    public int CalcCreatureDefense(Player pc, Creature crt)
        {
            int defense = crt.AC;
            if (crt.isHeld())
            {
        	    defense -= 4;
        	    gv.cc.addFloatyText(new Coordinate(crt.combatLocX, crt.combatLocY), "+4 att", "green");
            }            
            return defense;
        }
	    public int CalcPcDamageToCreature(Player pc, Creature crt)
        {
            int damModifier = 0;
            int adder = 0;
            bool melee = false;
            if ((mod.getItemByResRefForInfo(pc.MainHandRefs.resref).category.Equals("Melee")) 
        		    || (pc.MainHandRefs.name.Equals("none"))
        		    || (mod.getItemByResRefForInfo(pc.AmmoRefs.resref).name.Equals("none")))
            {
        	    melee = true;
        	    damModifier = (pc.strength - 10) / 2;
        	    //if has critical strike trait use dexterity for damage modifier in melee if greater than strength modifier
        	    if (gv.sf.hasTrait(pc, "criticalstrike"))
        	    {
        		    int damModifierDex = (pc.dexterity - 10) / 4;    
        		    if (damModifierDex > damModifier)
            	    {
        			    damModifier = (pc.dexterity - 10) / 2;
            	    }
        	    }
                
                if (IsAttackFromBehind(pc, crt))
                {
                    damModifier += mod.attackFromBehindDamageModifier;
                    if (mod.attackFromBehindDamageModifier > 0)
                    {
                        gv.cc.addLogText("<font color='lime'> Attack from behind: +" + mod.attackFromBehindDamageModifier.ToString() +  " damage." + "</font><BR>");
                    }
                }


            }
            else //ranged weapon used
            {
                damModifier = 0;  
                if (gv.sf.hasTrait(pc, "preciseshot2"))
	    	    {
	    		    damModifier += 2;
	    		    gv.cc.addLogText("<font color='lime'> PreciseShotL2: +2 damage</font><BR>");
	    	    }
                else if (gv.sf.hasTrait(pc, "preciseshot"))
	    	    {
            	    damModifier++;
            	    gv.cc.addLogText("<font color='lime'> PreciseShotL1: +1 damage</font><BR>");
	    	    }
	    	
            }            

            int dDam = mod.getItemByResRefForInfo(pc.MainHandRefs.resref).damageDie;
            float damage = (mod.getItemByResRefForInfo(pc.MainHandRefs.resref).damageNumDice * gv.sf.RandInt(dDam)) + damModifier + adder + mod.getItemByResRefForInfo(pc.MainHandRefs.resref).damageAdder;
            if (damage < 0)
            {
                damage = 0;
            }
            Item it = mod.getItemByResRefForInfo(pc.AmmoRefs.resref);
            if (it != null)
            {
        	    damage += mod.getItemByResRefForInfo(pc.AmmoRefs.resref).damageAdder;
            }

            float resist = 0;

            if (mod.getItemByResRefForInfo(pc.MainHandRefs.resref).typeOfDamage.Equals("Acid"))
            {
                resist = (float)(1f - ((float)crt.damageTypeResistanceValueAcid / 100f));
            }
            else if (mod.getItemByResRefForInfo(pc.MainHandRefs.resref).typeOfDamage.Equals("Normal"))
            {
                resist = (float)(1f - ((float)crt.damageTypeResistanceValueNormal / 100f));
            }
            else if (mod.getItemByResRefForInfo(pc.MainHandRefs.resref).typeOfDamage.Equals("Cold"))
            {
                resist = (float)(1f - ((float)crt.damageTypeResistanceValueCold / 100f));
            }
            else if (mod.getItemByResRefForInfo(pc.MainHandRefs.resref).typeOfDamage.Equals("Electricity"))
            {
                resist = (float)(1f - ((float)crt.damageTypeResistanceValueElectricity / 100f));
            }
            else if (mod.getItemByResRefForInfo(pc.MainHandRefs.resref).typeOfDamage.Equals("Fire"))
            {
                resist = (float)(1f - ((float)crt.damageTypeResistanceValueFire / 100f));
            }
            else if (mod.getItemByResRefForInfo(pc.MainHandRefs.resref).typeOfDamage.Equals("Magic"))
            {
                resist = (float)(1f - ((float)crt.damageTypeResistanceValueMagic / 100f));
            }
            else if (mod.getItemByResRefForInfo(pc.MainHandRefs.resref).typeOfDamage.Equals("Poison"))
            {
                resist = (float)(1f - ((float)crt.damageTypeResistanceValuePoison / 100f));
            }        

            int totalDam = (int)(damage * resist);
            if (totalDam < 0)
            {
                totalDam = 0;
            }
            //if doing sneak attack, does extra damage
            if ((pc.steathModeOn) && (melee) && (IsAttackFromBehind(pc,crt)))
            {
	            if (pc.knownTraitsTags.Contains("sneakattack"))
			    {
	        	    //+1d6 for every 2 levels after level 1
	        	    int multiplier = ((pc.classLevel - 1) / 2) + 1;
	        	    int adding = 0;
	        	    for (int i = 0; i < multiplier; i++)
	        	    {
	        		    adding += gv.sf.RandInt(6);
	        	    }
				    totalDam += adding;
				    gv.cc.addLogText("<font color='lime'> sneak attack: +" + adding + " damage</font><BR>");
			    }		        
            }
            return totalDam;
        }
	    public int CalcCreatureAttackModifier(Creature crt, Player pc)
        {
            if ((crt.cr_category.Equals("Ranged")) && (isAdjacentPc(crt)))
		    {
			    gv.cc.addLogText("<font color='yellow'>" + "-4 ranged attack penalty" + "</font>" +
        			    "<BR>");
        	    gv.cc.addLogText("<font color='yellow'>" + "with enemies in melee range" + "</font>" +
        			    "<BR>");
        	    gv.cc.addFloatyText(new Coordinate(crt.combatLocX, crt.combatLocY), "-4 att", "yellow");
        	    return crt.cr_att - 4; 
            }
            else //melee weapon used
            {
                int modifier = 0;
                //all attacks of the Creature from behind get a +2 bonus to hit            
                if (IsCreatureAttackFromBehind(pc, crt))
                {
                    modifier += 2;
                    gv.cc.addLogText("<font color='yellow'>" + crt.cr_name + " attacks from behind: +2 att</font><BR>");
                }
        	    return crt.cr_att + modifier;            
            }
        }
	    public int CalcPcDefense(Player pc, Creature crt)
        {
            int defense = pc.AC;
            if (pc.isHeld())
            {
        	    defense -= 4;
        	    gv.cc.addFloatyText(new Coordinate(pc.combatLocX, pc.combatLocY), "+4 att", "yellow");
            }
            return defense;
        }
        public int CalcCreatureDamageToPc(Player pc, Creature crt)
        {
            int dDam = crt.cr_damageDie;
            float damage = (crt.cr_damageNumDice * gv.sf.RandInt(dDam)) + crt.cr_damageAdder;
            if (damage < 0)
            {
                damage = 0;
            }

            float resist = 0;

            if (crt.cr_typeOfDamage.Equals("Acid"))
            {
                resist = (float)(1f - ((float)pc.damageTypeResistanceTotalAcid / 100f));
            }
            else if (crt.cr_typeOfDamage.Equals("Normal"))
            {
                resist = (float)(1f - ((float)pc.damageTypeResistanceTotalNormal / 100f));
            }
            else if (crt.cr_typeOfDamage.Equals("Cold"))
            {
                resist = (float)(1f - ((float)pc.damageTypeResistanceTotalCold / 100f));
            }
            else if (crt.cr_typeOfDamage.Equals("Electricity"))
            {
                resist = (float)(1f - ((float)pc.damageTypeResistanceTotalElectricity / 100f));
            }
            else if (crt.cr_typeOfDamage.Equals("Fire"))
            {
                resist = (float)(1f - ((float)pc.damageTypeResistanceTotalFire / 100f));
            }
            else if (crt.cr_typeOfDamage.Equals("Magic"))
            {
                resist = (float)(1f - ((float)pc.damageTypeResistanceTotalMagic / 100f));
            }
            else if (crt.cr_typeOfDamage.Equals("Poison"))
            {
                resist = (float)(1f - ((float)pc.damageTypeResistanceTotalPoison / 100f));
            }  

            int totalDam = (int)(damage * resist);
            if (totalDam < 0)
            {
                totalDam = 0;
            }

            return totalDam;
        }

        public Player targetClosestPC(Creature crt)
        {
            Player pc = null;
            int farDist = 99;
            foreach (Player p in mod.playerList)
            {
                if ((!p.isDead()) && (p.hp >= 0) && (!p.steathModeOn))
                {
                    int dist = CalcDistance(crt.combatLocX, crt.combatLocY, p.combatLocX, p.combatLocY);
                    if (dist == farDist)
                    {
                	    //since at same distance, do a random check to see if switch or stay with current PC target
                	    if (gv.sf.RandInt(20) > 10)
                	    {
                		    //switch target
                		    pc = p;
                		    if (gv.mod.debugMode)
						    {
                			    gv.cc.addLogText("<font color='yellow'>target:" + pc.name + "</font><BR>");
						    }
                	    }                	
                    }
                    else if (dist < farDist)
                    {
                        farDist = dist;
                        pc = p;
                        if (gv.mod.debugMode)
					    {
            			    gv.cc.addLogText("<font color='yellow'>target:" + pc.name + "</font><BR>");
					    }
                    }
                }
            }
            return pc;
        }
        public Coordinate targetBestPointLocation(Creature crt)
        {
            Coordinate targetLoc = new Coordinate(-1,-1);
            //JamesManhattan Utility maximization function for the VERY INTELLIGENT CREATURE CASTER
            int utility = 0; //utility
            int optimalUtil = 0; //optimal utility, a storage of the highest achieved
            Coordinate selectedPoint = new Coordinate(crt.combatLocX, crt.combatLocY); //Initial Select Point is Creature itself, then loop through all squares within range!
            for (int y = gv.sf.SpellToCast.range; y > -gv.sf.SpellToCast.range; y--)  //start at far range and work backwards does a pretty good job of avoiding hitting allies.
            {
                for (int x = gv.sf.SpellToCast.range; x > -gv.sf.SpellToCast.range; x--)
                {
                    utility = 0; //reset utility for each point tested
                    selectedPoint = new Coordinate(crt.combatLocX + x, crt.combatLocY + y);
                
                    //check if selected point is a valid location on combat map
                    if ((selectedPoint.X < 0) || (selectedPoint.X > mod.currentEncounter.MapSizeX - 1) || (selectedPoint.Y < 0) || (selectedPoint.Y > mod.currentEncounter.MapSizeY - 1))
                    {
                	    continue;
                    }
                
                    //check if selected point is in LoS, if not skip this point
                    int endX = selectedPoint.X * gv.squareSize + (gv.squareSize / 2);
        		    int endY = selectedPoint.Y * gv.squareSize + (gv.squareSize / 2);
        		    int startX = crt.combatLocX * gv.squareSize + (gv.squareSize / 2);
        		    int startY = crt.combatLocY * gv.squareSize + (gv.squareSize / 2);
                    if (!isVisibleLineOfSight(new Coordinate(endX, endY), new Coordinate(startX, startY)))
                    {
                	    continue;
                    }
                
                    if (selectedPoint == new Coordinate(crt.combatLocX, crt.combatLocY))
                    {
                        utility -= 4; //the creature at least attempts to avoid hurting itself, but if surrounded might fireball itself!
                        if (crt.hp <= crt.hpMax / 4) //caster is wounded, definately avoids itself.
                        {
                            utility -= 4;
                        }
                    } 
                    foreach (Creature crtr in mod.currentEncounter.encounterCreatureList) //if its allies are in the burst subtract a point, or half depending on how evil it is.
                    {
                        if (this.CalcDistance(crtr.combatLocX, crtr.combatLocY, selectedPoint.X, selectedPoint.Y) <= gv.sf.SpellToCast.aoeRadius) //if friendly creatures are in the AOE burst, count how many, subtract 0.5 for each, evil is evil
                        {
                            utility -= 1;
                        }
                    }
                    foreach (Player tgt_pc in mod.playerList)
                    {
                        if ((this.CalcDistance(tgt_pc.combatLocX, tgt_pc.combatLocY, selectedPoint.X, selectedPoint.Y) <= gv.sf.SpellToCast.aoeRadius) && (tgt_pc.hp > 0)) //if players are in the AOE burst, count how many, total count is utility  //&& sf.GetLocalInt(tgt_pc.Tag, "StealthModeOn") != 1  <-throws an annoying message if not found!!
                        {
                            utility += 2;
                            if (utility > optimalUtil)
                            {
                                //optimal found, choose this point
                                optimalUtil = utility;
                                targetLoc = selectedPoint;
                            }
                        }
                    }
                    if (gv.mod.debugMode)
				    {
        			    gv.cc.addLogText("<font color='yellow'>(" + selectedPoint.X + "," + selectedPoint.Y + "):" + utility + "</font><BR>");
				    }
                }
            }

            return targetLoc;
        }
	    public int CalcDistance(int locCrX, int locCrY, int locPcX, int locPcY)
        {
            int dist = 0;
            int deltaX = (int)Math.Abs((locCrX - locPcX));
            int deltaY = (int)Math.Abs((locCrY - locPcY));
            if (deltaX > deltaY)
                dist = deltaX;
            else
                dist = deltaY;
            return dist;
        }
	    public Creature GetCreatureWithLowestHP()
        {
            int lowHP = 999;
            Creature returnCrt = null;
            foreach (Creature crt in mod.currentEncounter.encounterCreatureList)
            {
                if (crt.hp > 0)
                {
                    if (crt.hp < lowHP)
                    {
                        lowHP = crt.hp;
                        returnCrt = crt;
                    }
                }
            }
            return returnCrt;
        }
	    public Creature GetCreatureWithMostDamaged()
        {
            int damaged = 0;
            Creature returnCrt = null;
            foreach (Creature crt in mod.currentEncounter.encounterCreatureList)
            {        	
                if (crt.hp > 0)
                {
            	    int dam = crt.hpMax - crt.hp;
                    if (dam > damaged)
                    {
                	    damaged = dam;
                        returnCrt = crt;
                    }
                }
            }
            return returnCrt;
        }
	    public Creature GetNextAdjacentCreature(Player pc)
        {
            foreach (Creature nextCrt in mod.currentEncounter.encounterCreatureList)
            {
                if ((CalcDistance(nextCrt.combatLocX, nextCrt.combatLocY, pc.combatLocX, pc.combatLocY) < 2) && (nextCrt.hp > 0))
                {
                    return nextCrt;
                }
            }
            return null;
        }
	    public Creature GetCreatureByTag(String tag)
        {
            foreach (Creature crt in mod.currentEncounter.encounterCreatureList)
            {
                if (crt.cr_tag.Equals(tag))
                {
                    return crt;
                }
            }
            return null;
        }
    }
}
