using System;
using Server.Items;
using Server.Spells;
using Server.ACC.CSS.Systems.AlchemyMagic;
using Server.ACC.CSS.Systems.FishingMagic;
using Server.ACC.CSS.Systems.EvalIntMagic;
using Server.ACC.CSS.Systems.ArcheryMagic;
using Server.ACC.CSS.Systems.MageryMagic;
using Server.ACC.CSS.Systems.ArmsLoreMagic;
using Server.ACC.CSS.Systems.AnimalTamingMagic;
using Server.ACC.CSS.Systems.AnimalLoreMagic;
using Server.ACC.CSS.Systems.CarpentryMagic;
using Server.ACC.CSS.Systems.CartographyMagic;
using Server.ACC.CSS.Systems.TasteIDMagic;
using Server.ACC.CSS.Systems.CookingMagic;
using Server.ACC.CSS.Systems.DiscordanceMagic;
using Server.ACC.CSS.Systems.FencingMagic;
using Server.ACC.CSS.Systems.FletchingMagic;
using Server.ACC.CSS.Systems.ForensicsMagic;
using Server.ACC.CSS.Systems.WrestlingMagic;
using Server.ACC.CSS.Systems.ParryMagic;
using Server.ACC.CSS.Systems.HealingMagic;
using Server.ACC.CSS.Systems.DetectHiddenMagic;
using Server.ACC.CSS.Systems.ProvocationMagic;
using Server.ACC.CSS.Systems.LockpickingMagic;
using Server.ACC.CSS.Systems.MacingMagic;
using Server.ACC.CSS.Systems.MeditationMagic;
using Server.ACC.CSS.Systems.BeggingMagic;
using Server.ACC.CSS.Systems.MiningMagic;
using Server.ACC.CSS.Systems.ChivalryMagic;
using Server.ACC.CSS.Systems.StealingMagic;
using Server.ACC.CSS.Systems.InscribeMagic;
using Server.ACC.CSS.Systems.NinjitsuMagic;
using Server.ACC.CSS.Systems.HidingMagic;
using Server.ACC.CSS.Systems.StealthMagic;
using Server.ACC.CSS.Systems.BlacksmithMagic;
using Server.ACC.CSS.Systems.TacticsMagic;
using Server.ACC.CSS.Systems.SwordsMagic;
using Server.ACC.CSS.Systems.TailoringMagic;
using Server.ACC.CSS.Systems.NecromancyMagic;
using Server.ACC.CSS.Systems.TrackingMagic;
using Server.ACC.CSS.Systems.RemoveTrapMagic;
using Server.ACC.CSS.Systems.VeterinaryMagic;
using Server.ACC.CSS.Systems.MusicianshipMagic;
using Server.ACC.CSS.Systems.CampingMagic;
using Server.ACC.CSS.Systems.LumberjackingMagic;
using Server.ACC.CSS.Systems.SpiritSpeakMagic;

namespace Server.Engines.Craft
{
    public enum InscriptionRecipes
    {
        RunicAtlas = 800
    }

    public class DefInscription : CraftSystem
    {
        public override SkillName MainSkill
        {
            get
            {
                return SkillName.Inscribe;
            }
        }

        public override int GumpTitleNumber
        {
            get
            {
                return 1044009;
            }// <CENTER>INSCRIPTION MENU</CENTER>
        }

        private static CraftSystem m_CraftSystem;

        public static CraftSystem CraftSystem
        {
            get
            {
                if (m_CraftSystem == null)
                    m_CraftSystem = new DefInscription();

                return m_CraftSystem;
            }
        }

        public override double GetChanceAtMin(CraftItem item)
        {
            return 0.0; // 0%
        }

        private DefInscription()
            : base(1, 1, 1.25)// base( 1, 1, 3.0 )
        {
        }

        public override int CanCraft(Mobile from, ITool tool, Type typeItem)
        {
            int num = 0;

            if (tool == null || tool.Deleted || tool.UsesRemaining <= 0)
                return 1044038; // You have worn out your tool!
            else if (!tool.CheckAccessible(from, ref num))
                return num; // The tool must be on your person to use.

            if (typeItem != null && typeItem.IsSubclassOf(typeof(SpellScroll)))
            {
                if (!_Buffer.ContainsKey(typeItem))
                {
                    object o = Activator.CreateInstance(typeItem);

                    if (o is SpellScroll)
                    {
                        SpellScroll scroll = (SpellScroll)o;
                        _Buffer[typeItem] = scroll.SpellID;
                        scroll.Delete();
                    }
                    else if (o is IEntity)
                    {
                        ((IEntity)o).Delete();
                        return 1042404; // You don't have that spell!
                    }
                }

                int id = _Buffer[typeItem];
                Spellbook book = Spellbook.Find(from, id);

                if (book == null || !book.HasSpell(id))
                    return 1042404; // You don't have that spell!
            }

            return 0;
        }

        private System.Collections.Generic.Dictionary<Type, int> _Buffer = new System.Collections.Generic.Dictionary<Type, int>();

        public override void PlayCraftEffect(Mobile from)
        {
            from.PlaySound(0x249);
        }

        private static readonly Type typeofSpellScroll = typeof(SpellScroll);

        public override int PlayEndingEffect(Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality, bool makersMark, CraftItem item)
        {
            if (toolBroken)
                from.SendLocalizedMessage(1044038); // You have worn out your tool

            if (!typeofSpellScroll.IsAssignableFrom(item.ItemType)) //  not a scroll
            {
                if (failed)
                {
                    if (lostMaterial)
                        return 1044043; // You failed to create the item, and some of your materials are lost.
                    else
                        return 1044157; // You failed to create the item, but no materials were lost.
                }
                else
                {
                    if (quality == 0)
                        return 502785; // You were barely able to make this item.  It's quality is below average.
                    else if (makersMark && quality == 2)
                        return 1044156; // You create an exceptional quality item and affix your maker's mark.
                    else if (quality == 2)
                        return 1044155; // You create an exceptional quality item.
                    else
                        return 1044154; // You create the item.
                }
            }
            else
            {
                if (failed)
                    return 501630; // You fail to inscribe the scroll, and the scroll is ruined.
                else
                    return 501629; // You inscribe the spell and put the scroll in your backpack.
            }
        }

        private int m_Circle, m_Mana;

        private enum Reg { BlackPearl, Bloodmoss, Garlic, Ginseng, MandrakeRoot, Nightshade, SulfurousAsh, SpidersSilk, BatWing, GraveDust, DaemonBlood, NoxCrystal, PigIron, Bone, DragonBlood, FertileDirt, DaemonBone }

        private readonly Type[] m_RegTypes = new Type[]
        {
            typeof( BlackPearl ),
            typeof( Bloodmoss ),
            typeof( Garlic ),
            typeof( Ginseng ),
            typeof( MandrakeRoot ),
            typeof( Nightshade ),
            typeof( SulfurousAsh ),
            typeof( SpidersSilk ),
            typeof( BatWing ),
            typeof( GraveDust ),
            typeof( DaemonBlood ),
            typeof( NoxCrystal ),
            typeof( PigIron ),
            typeof( Bone ),
            typeof( DragonBlood ),
            typeof( FertileDirt ),
            typeof( DaemonBone )
        };

        private int m_Index;

        private void AddSpell(Type type, params Reg[] regs)
        {
            double minSkill, maxSkill;
            int cliloc;

            switch (m_Circle)
            {
                default:
                case 0: minSkill = -25.0; maxSkill = 25.0; cliloc = 1111691; break;
                case 1: minSkill = -10.8; maxSkill = 39.2; cliloc = 1111691; break;
                case 2: minSkill = 03.5; maxSkill = 53.5; cliloc = 1111692; break;
                case 3: minSkill = 17.8; maxSkill = 67.8; cliloc = 1111692; break;
                case 4: minSkill = 32.1; maxSkill = 82.1; cliloc = 1111693; break;
                case 5: minSkill = 46.4; maxSkill = 96.4; cliloc = 1111693; break;
                case 6: minSkill = 60.7; maxSkill = 110.7; cliloc = 1111694; break;
                case 7: minSkill = 75.0; maxSkill = 125.0; cliloc = 1111694; break;
            }

            int index = AddCraft(type, cliloc, 1044381 + m_Index++, minSkill, maxSkill, m_RegTypes[(int)regs[0]], 1044353 + (int)regs[0], 1, 1044361 + (int)regs[0]);

            for (int i = 1; i < regs.Length; ++i)
                AddRes(index, m_RegTypes[(int)regs[i]], 1044353 + (int)regs[i], 1, 1044361 + (int)regs[i]);

            AddRes(index, typeof(BlankScroll), 1044377, 1, 1044378);

            SetManaReq(index, m_Mana);
        }

        private void AddNecroSpell(int spell, int mana, double minSkill, Type type, params Reg[] regs)
        {
            int id = GetRegLocalization(regs[0]);
            int index = AddCraft(type, 1061677, 1060509 + spell, minSkill, minSkill + 1.0, m_RegTypes[(int)regs[0]], id, 1, 501627);

            for (int i = 1; i < regs.Length; ++i)
            {
                id = GetRegLocalization(regs[i]);
                AddRes(index, m_RegTypes[(int)regs[i]], id, 1, 501627);
            }

            AddRes(index, typeof(BlankScroll), 1044377, 1, 1044378);

            SetManaReq(index, mana);
        }

        private void AddMysticSpell(int id, int mana, double minSkill, Type type, params Reg[] regs)
        {
            int index = AddCraft(type, 1111671, id, minSkill, minSkill + 1.0, m_RegTypes[(int)regs[0]], GetRegLocalization(regs[0]), 1, 501627);	//Yes, on OSI it's only 1.0 skill diff'.  Don't blame me, blame OSI.

            for (int i = 1; i < regs.Length; ++i)
                AddRes(index, m_RegTypes[(int)regs[i]], GetRegLocalization(regs[i]), 1, 501627);

            AddRes(index, typeof(BlankScroll), 1044377, 1, 1044378);

            SetManaReq(index, mana);
        }

        private int GetRegLocalization(Reg reg)
        {
            int loc = 0;

            switch (reg)
            {
                case Reg.BatWing: loc = 1023960; break;
                case Reg.GraveDust: loc = 1023983; break;
                case Reg.DaemonBlood: loc = 1023965; break;
                case Reg.NoxCrystal: loc = 1023982; break;
                case Reg.PigIron: loc = 1023978; break;
                case Reg.Bone: loc = 1023966; break;
                case Reg.DragonBlood: loc = 1023970; break;
                case Reg.FertileDirt: loc = 1023969; break;
                case Reg.DaemonBone: loc = 1023968; break;
            }

            if (loc == 0)
                loc = 1044353 + (int)reg;

            return loc;
        }

        public override void InitCraftList()
        {
            m_Circle = 0;
            m_Mana = 4;

            AddSpell(typeof(ReactiveArmorScroll), Reg.Garlic, Reg.SpidersSilk, Reg.SulfurousAsh);
            AddSpell(typeof(ClumsyScroll), Reg.Bloodmoss, Reg.Nightshade);
            AddSpell(typeof(CreateFoodScroll), Reg.Garlic, Reg.Ginseng, Reg.MandrakeRoot);
            AddSpell(typeof(FeeblemindScroll), Reg.Nightshade, Reg.Ginseng);
            AddSpell(typeof(HealScroll), Reg.Garlic, Reg.Ginseng, Reg.SpidersSilk);
            AddSpell(typeof(MagicArrowScroll), Reg.SulfurousAsh);
            AddSpell(typeof(NightSightScroll), Reg.SpidersSilk, Reg.SulfurousAsh);
            AddSpell(typeof(WeakenScroll), Reg.Garlic, Reg.Nightshade);

            m_Circle = 1;
            m_Mana = 6;

            AddSpell(typeof(AgilityScroll), Reg.Bloodmoss, Reg.MandrakeRoot);
            AddSpell(typeof(CunningScroll), Reg.Nightshade, Reg.MandrakeRoot);
            AddSpell(typeof(CureScroll), Reg.Garlic, Reg.Ginseng);
            AddSpell(typeof(HarmScroll), Reg.Nightshade, Reg.SpidersSilk);
            AddSpell(typeof(MagicTrapScroll), Reg.Garlic, Reg.SpidersSilk, Reg.SulfurousAsh);
            AddSpell(typeof(MagicUnTrapScroll), Reg.Bloodmoss, Reg.SulfurousAsh);
            AddSpell(typeof(ProtectionScroll), Reg.Garlic, Reg.Ginseng, Reg.SulfurousAsh);
            AddSpell(typeof(StrengthScroll), Reg.Nightshade, Reg.MandrakeRoot);

            m_Circle = 2;
            m_Mana = 9;

            AddSpell(typeof(BlessScroll), Reg.Garlic, Reg.MandrakeRoot);
            AddSpell(typeof(FireballScroll), Reg.BlackPearl);
            AddSpell(typeof(MagicLockScroll), Reg.Bloodmoss, Reg.Garlic, Reg.SulfurousAsh);
            AddSpell(typeof(PoisonScroll), Reg.Nightshade);
            AddSpell(typeof(TelekinisisScroll), Reg.Bloodmoss, Reg.MandrakeRoot);
            AddSpell(typeof(TeleportScroll), Reg.Bloodmoss, Reg.MandrakeRoot);
            AddSpell(typeof(UnlockScroll), Reg.Bloodmoss, Reg.SulfurousAsh);
            AddSpell(typeof(WallOfStoneScroll), Reg.Bloodmoss, Reg.Garlic);

            m_Circle = 3;
            m_Mana = 11;

            AddSpell(typeof(ArchCureScroll), Reg.Garlic, Reg.Ginseng, Reg.MandrakeRoot);
            AddSpell(typeof(ArchProtectionScroll), Reg.Garlic, Reg.Ginseng, Reg.MandrakeRoot, Reg.SulfurousAsh);
            AddSpell(typeof(CurseScroll), Reg.Garlic, Reg.Nightshade, Reg.SulfurousAsh);
            AddSpell(typeof(FireFieldScroll), Reg.BlackPearl, Reg.SpidersSilk, Reg.SulfurousAsh);
            AddSpell(typeof(GreaterHealScroll), Reg.Garlic, Reg.SpidersSilk, Reg.MandrakeRoot, Reg.Ginseng);
            AddSpell(typeof(LightningScroll), Reg.MandrakeRoot, Reg.SulfurousAsh);
            AddSpell(typeof(ManaDrainScroll), Reg.BlackPearl, Reg.SpidersSilk, Reg.MandrakeRoot);
            AddSpell(typeof(RecallScroll), Reg.BlackPearl, Reg.Bloodmoss, Reg.MandrakeRoot);

            m_Circle = 4;
            m_Mana = 14;

            AddSpell(typeof(BladeSpiritsScroll), Reg.BlackPearl, Reg.Nightshade, Reg.MandrakeRoot);
            AddSpell(typeof(DispelFieldScroll), Reg.BlackPearl, Reg.Garlic, Reg.SpidersSilk, Reg.SulfurousAsh);
            AddSpell(typeof(IncognitoScroll), Reg.Bloodmoss, Reg.Garlic, Reg.Nightshade);
            AddSpell(typeof(MagicReflectScroll), Reg.Garlic, Reg.MandrakeRoot, Reg.SpidersSilk);
            AddSpell(typeof(MindBlastScroll), Reg.BlackPearl, Reg.MandrakeRoot, Reg.Nightshade, Reg.SulfurousAsh);
            AddSpell(typeof(ParalyzeScroll), Reg.Garlic, Reg.MandrakeRoot, Reg.SpidersSilk);
            AddSpell(typeof(PoisonFieldScroll), Reg.BlackPearl, Reg.Nightshade, Reg.SpidersSilk);
            AddSpell(typeof(SummonCreatureScroll), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SpidersSilk);

            m_Circle = 5;
            m_Mana = 20;

            AddSpell(typeof(DispelScroll), Reg.Garlic, Reg.MandrakeRoot, Reg.SulfurousAsh);
            AddSpell(typeof(EnergyBoltScroll), Reg.BlackPearl, Reg.Nightshade);
            AddSpell(typeof(ExplosionScroll), Reg.Bloodmoss, Reg.MandrakeRoot);
            AddSpell(typeof(InvisibilityScroll), Reg.Bloodmoss, Reg.Nightshade);
            AddSpell(typeof(MarkScroll), Reg.Bloodmoss, Reg.BlackPearl, Reg.MandrakeRoot);
            AddSpell(typeof(MassCurseScroll), Reg.Garlic, Reg.MandrakeRoot, Reg.Nightshade, Reg.SulfurousAsh);
            AddSpell(typeof(ParalyzeFieldScroll), Reg.BlackPearl, Reg.Ginseng, Reg.SpidersSilk);
            AddSpell(typeof(RevealScroll), Reg.Bloodmoss, Reg.SulfurousAsh);

            m_Circle = 6;
            m_Mana = 40;

            AddSpell(typeof(ChainLightningScroll), Reg.BlackPearl, Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SulfurousAsh);
            AddSpell(typeof(EnergyFieldScroll), Reg.BlackPearl, Reg.MandrakeRoot, Reg.SpidersSilk, Reg.SulfurousAsh);
            AddSpell(typeof(FlamestrikeScroll), Reg.SpidersSilk, Reg.SulfurousAsh);
            AddSpell(typeof(GateTravelScroll), Reg.BlackPearl, Reg.MandrakeRoot, Reg.SulfurousAsh);
            AddSpell(typeof(ManaVampireScroll), Reg.BlackPearl, Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SpidersSilk);
            AddSpell(typeof(MassDispelScroll), Reg.BlackPearl, Reg.Garlic, Reg.MandrakeRoot, Reg.SulfurousAsh);
            AddSpell(typeof(MeteorSwarmScroll), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SulfurousAsh, Reg.SpidersSilk);
            AddSpell(typeof(PolymorphScroll), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SpidersSilk);

            m_Circle = 7;
            m_Mana = 50;

            AddSpell(typeof(EarthquakeScroll), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.Ginseng, Reg.SulfurousAsh);
            AddSpell(typeof(EnergyVortexScroll), Reg.BlackPearl, Reg.Bloodmoss, Reg.MandrakeRoot, Reg.Nightshade);
            AddSpell(typeof(ResurrectionScroll), Reg.Bloodmoss, Reg.Garlic, Reg.Ginseng);
            AddSpell(typeof(SummonAirElementalScroll), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SpidersSilk);
            AddSpell(typeof(SummonDaemonScroll), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SpidersSilk, Reg.SulfurousAsh);
            AddSpell(typeof(SummonEarthElementalScroll), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SpidersSilk);
            AddSpell(typeof(SummonFireElementalScroll), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SpidersSilk, Reg.SulfurousAsh);
            AddSpell(typeof(SummonWaterElementalScroll), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SpidersSilk);

            if (Core.SE)
            {
                AddNecroSpell(0, 23, 39.6, typeof(AnimateDeadScroll), Reg.GraveDust, Reg.DaemonBlood);
                AddNecroSpell(1, 13, 19.6, typeof(BloodOathScroll), Reg.DaemonBlood);
                AddNecroSpell(2, 11, 19.6, typeof(CorpseSkinScroll), Reg.BatWing, Reg.GraveDust);
                AddNecroSpell(3, 7, 19.6, typeof(CurseWeaponScroll), Reg.PigIron);
                AddNecroSpell(4, 11, 19.6, typeof(EvilOmenScroll), Reg.BatWing, Reg.NoxCrystal);
                AddNecroSpell(5, 11, 39.6, typeof(HorrificBeastScroll), Reg.BatWing, Reg.DaemonBlood);
                AddNecroSpell(6, 23, 69.6, typeof(LichFormScroll), Reg.GraveDust, Reg.DaemonBlood, Reg.NoxCrystal);
                AddNecroSpell(7, 17, 29.6, typeof(MindRotScroll), Reg.BatWing, Reg.DaemonBlood, Reg.PigIron);
                AddNecroSpell(8, 5, 19.6, typeof(PainSpikeScroll), Reg.GraveDust, Reg.PigIron);
                AddNecroSpell(9, 17, 49.6, typeof(PoisonStrikeScroll), Reg.NoxCrystal);
                AddNecroSpell(10, 29, 64.6, typeof(StrangleScroll), Reg.DaemonBlood, Reg.NoxCrystal);
                AddNecroSpell(11, 17, 29.6, typeof(SummonFamiliarScroll), Reg.BatWing, Reg.GraveDust, Reg.DaemonBlood);
                AddNecroSpell(12, 23, 98.6, typeof(VampiricEmbraceScroll), Reg.BatWing, Reg.NoxCrystal, Reg.PigIron);
                AddNecroSpell(13, 41, 79.6, typeof(VengefulSpiritScroll), Reg.BatWing, Reg.GraveDust, Reg.PigIron);
                AddNecroSpell(14, 23, 59.6, typeof(WitherScroll), Reg.GraveDust, Reg.NoxCrystal, Reg.PigIron);
                AddNecroSpell(15, 17, 19.6, typeof(WraithFormScroll), Reg.NoxCrystal, Reg.PigIron);
                AddNecroSpell(16, 40, 79.6, typeof(ExorcismScroll), Reg.NoxCrystal, Reg.GraveDust);
            }

            int index;

            if (Core.ML)
            {
                index = AddCraft(typeof(EnchantedSwitch), 1044294, 1072893, 45.0, 95.0, typeof(BlankScroll), 1044377, 1, 1044378);
                AddRes(index, typeof(SpidersSilk), 1044360, 1, 1044253);
                AddRes(index, typeof(BlackPearl), 1044353, 1, 1044253);
                AddRes(index, typeof(SwitchItem), 1073464, 1, 1044253);
                ForceNonExceptional(index);

                index = AddCraft(typeof(RunedPrism), 1044294, 1073465, 45.0, 95.0, typeof(BlankScroll), 1044377, 1, 1044378);
                AddRes(index, typeof(SpidersSilk), 1044360, 1, 1044253);
                AddRes(index, typeof(BlackPearl), 1044353, 1, 1044253);
                AddRes(index, typeof(HollowPrism), 1072895, 1, 1044253);
                ForceNonExceptional(index);
            }

            // Runebook
            index = AddCraft(typeof(Runebook), 1044294, 1041267, 45.0, 95.0, typeof(BlankScroll), 1044377, 8, 1044378);
            AddRes(index, typeof(RecallScroll), 1044445, 1, 1044253);
            AddRes(index, typeof(GateTravelScroll), 1044446, 1, 1044253);

            #region TOL
            if (Core.TOL)
            {
                index = AddCraft(typeof(RunicAtlas), 1044294, 1156443, 45.0, 95.0, typeof(BlankScroll), 1044377, 24, 1044378);
                AddRes(index, typeof(RecallRune), 1044447, 3, 1044253);
                AddRes(index, typeof(RecallScroll), 1044445, 3, 1044253);
                AddRes(index, typeof(GateTravelScroll), 1044446, 3, 1044253);
                AddRecipe(index, (int)InscriptionRecipes.RunicAtlas);
            }
            #endregion

            if (Core.AOS)
            {
                AddCraft(typeof(Engines.BulkOrders.BulkOrderBook), 1044294, 1028793, 65.0, 115.0, typeof(BlankScroll), 1044377, 10, 1044378);
            }

            if (Core.SE)
            {
                AddCraft(typeof(Spellbook), 1044294, 1023834, 50.0, 126, typeof(BlankScroll), 1044377, 10, 1044378);
            }

            #region Mondain's Legacy	
            if (Core.ML)
            {
                index = AddCraft(typeof(ScrappersCompendium), 1044294, 1072940, 75.0, 125.0, typeof(BlankScroll), 1044377, 100, 1044378);
                AddRes(index, typeof(DreadHornMane), 1032682, 1, 1044253);
                AddRes(index, typeof(Taint), 1032679, 10, 1044253);
                AddRes(index, typeof(Corruption), 1032676, 10, 1044253);
                AddRecipe(index, (int)TinkerRecipes.ScrappersCompendium);
                ForceNonExceptional(index);

                index = AddCraft(typeof(SpellbookEngraver), 1044294, 1072151, 75.0, 100.0, typeof(Feather), 1044562, 1, 1044563);
                AddRes(index, typeof(BlackPearl), 1015001, 7, 1044253);


                AddCraft(typeof(NecromancerSpellbook), 1044294, 1074909, 50.0, 100.0, typeof(BlankScroll), 1044377, 10, 1044378);

                AddCraft(typeof(MysticBook), 1044294, 1031677, 50.0, 100.0, typeof(BlankScroll), 1044377, 10, 1044378);
            }
            #endregion

            #region Stygian Abyss
            if (Core.SA)
            {
                index = AddCraft(typeof(ExodusSummoningRite), 1044294, 1153498, 95.0, 120.0, typeof(DaemonBlood), 1023965, 5, 1044253);
                AddRes(index, typeof(Taint), 1032679, 1, 1044253);
                AddRes(index, typeof(DaemonBone), 1017412, 5, 1044253);
                AddRes(index, typeof(SummonDaemonScroll), 1016017, 1, 1044253);

                index = AddCraft(typeof(PropheticManuscript), 1044294, 1155631, 90.0, 115.0, typeof(AncientParchment), 1155627, 10, 1044253);
                AddRes(index, typeof(AntiqueDocumentsKit), 1155630, 1, 1044253);
                AddRes(index, typeof(WoodPulp), 1113136, 10, 1113289);
                AddRes(index, typeof(Beeswax), 1025154, 5, 1044253);

                AddCraft(typeof(BlankScroll), 1044294, 1023636, 50.0, 100.0, typeof(WoodPulp), 1113136, 1, 1044378);

                index = AddCraft(typeof(ScrollBinderDeed), 1044294, 1113135, 75.0, 125.0, typeof(WoodPulp), 1113136, 1, 1044253);
                SetItemHue(index, 1641);

                index = AddCraft(typeof(GargoyleBook100), 1044294, 1113290, 60.0, 100.0, typeof(BlankScroll), 1044377, 40, 1044378);
                AddRes(index, typeof(Beeswax), 1025154, 2, "You do not have enough beeswax.");

                index = AddCraft(typeof(GargoyleBook200), 1044294, 1113291, 72.0, 100.0, typeof(BlankScroll), 1044377, 40, 1044378);
                AddRes(index, typeof(Beeswax), 1025154, 4, "You do not have enough beeswax.");

                AddMysticSpell(1031678, 4, 0.0, typeof(NetherBoltScroll), Reg.SulfurousAsh, Reg.BlackPearl);
                AddMysticSpell(1031679, 4, 0.0, typeof(HealingStoneScroll), Reg.Bone, Reg.Garlic, Reg.Ginseng, Reg.SpidersSilk);
                AddMysticSpell(1031680, 6, 0.0, typeof(PurgeMagicScroll), Reg.FertileDirt, Reg.Garlic, Reg.MandrakeRoot, Reg.SulfurousAsh);
                AddMysticSpell(1031681, 6, 0.0, typeof(EnchantScroll), Reg.SpidersSilk, Reg.MandrakeRoot, Reg.SulfurousAsh);
                AddMysticSpell(1031682, 9, 3.5, typeof(SleepScroll), Reg.SpidersSilk, Reg.BlackPearl, Reg.Nightshade);
                AddMysticSpell(1031683, 9, 3.5, typeof(EagleStrikeScroll), Reg.SpidersSilk, Reg.Bloodmoss, Reg.MandrakeRoot, Reg.Bone);
                AddMysticSpell(1031684, 11, 17.8, typeof(AnimatedWeaponScroll), Reg.Bone, Reg.BlackPearl, Reg.MandrakeRoot, Reg.Nightshade);
                AddMysticSpell(1031685, 11, 17.8, typeof(StoneFormScroll), Reg.Bloodmoss, Reg.FertileDirt, Reg.Garlic);
                AddMysticSpell(1031686, 14, 32.1, typeof(SpellTriggerScroll), Reg.SpidersSilk, Reg.MandrakeRoot, Reg.Garlic, Reg.DragonBlood);
                AddMysticSpell(1031687, 14, 32.1, typeof(MassSleepScroll), Reg.SpidersSilk, Reg.Nightshade, Reg.Ginseng);
                AddMysticSpell(1031688, 20, 46.4, typeof(CleansingWindsScroll), Reg.Ginseng, Reg.Garlic, Reg.DragonBlood, Reg.MandrakeRoot);
                AddMysticSpell(1031689, 20, 46.4, typeof(BombardScroll), Reg.Garlic, Reg.DragonBlood, Reg.SulfurousAsh, Reg.Bloodmoss);
                AddMysticSpell(1031690, 40, 60.7, typeof(SpellPlagueScroll), Reg.DaemonBone, Reg.DragonBlood, Reg.MandrakeRoot, Reg.Nightshade, Reg.SulfurousAsh, Reg.DaemonBone);
                AddMysticSpell(1031691, 40, 60.7, typeof(HailStormScroll), Reg.DragonBlood, Reg.BlackPearl, Reg.MandrakeRoot, Reg.Bloodmoss);
                AddMysticSpell(1031692, 50, 75.0, typeof(NetherCycloneScroll), Reg.Bloodmoss, Reg.Nightshade, Reg.SulfurousAsh, Reg.MandrakeRoot);
                AddMysticSpell(1031693, 50, 75.0, typeof(RisingColossusScroll), Reg.DaemonBone, Reg.FertileDirt, Reg.DragonBlood, Reg.Nightshade, Reg.MandrakeRoot);
            }
            #endregion
			//Power Deeds
            index = AddCraft(typeof(AdventurersContract), "Power Deeds", "Adventurers Contract", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(NegativePhocite), "NegativePhocite", 1, 1044253);
            AddRes(index, typeof(Fibrogen), "Fibrogen", 1, 1044253);
            index = AddCraft(typeof(ArmSlotChangeDeed), "Power Deeds", "ArmSlot Change Deed", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(OpaqueHydragyon), "OpaqueHydragyon", 1, 1044253);
            AddRes(index, typeof(ExoticEun), "ExoticEun", 1, 1044253);
            index = AddCraft(typeof(BeltSlotChangeDeed), "Power Deeds", "BeltSlot Change Deed", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Turbesium), "Turbesium", 1, 1044253);
            AddRes(index, typeof(Bloodglass), "Bloodglass", 1, 1044253);
            index = AddCraft(typeof(BraceletSlotChangeDeed), "Power Deeds", "BraceletSlot Change Deed", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(FlammablePlasmine), "FlammablePlasmine", 1, 1044253);
            AddRes(index, typeof(Thoril), "Thoril", 1, 1044253);
            index = AddCraft(typeof(CapacityIncreaseDeed), "Power Deeds", "CapacityIncrease Deed", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Bloodglass), "Bloodglass", 1, 1044253);
            AddRes(index, typeof(Uranimite), "Uranimite", 1, 1044253);
            index = AddCraft(typeof(EarringSlotChangeDeed), "Power Deeds", "EarringSlotChange Deed", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Flurocite), "Flurocite", 1, 1044253);
            AddRes(index, typeof(Impervanium), "Impervanium", 1, 1044253);
            index = AddCraft(typeof(FootwearSlotChangeDeed), "Power Deeds", "FootwearSlotChange Deed", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(ChargedAcoustesium), "ChargedAcoustesium", 1, 1044253);
            AddRes(index, typeof(Aeroglass), "Aeroglass", 1, 1044253);
            index = AddCraft(typeof(GenderChangeDeed), "Power Deeds", "genderswap deed", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(HighdensityElectron), "HighdensityElectron", 1, 1044253);
            AddRes(index, typeof(Negite), "Negite", 1, 1044253);
            index = AddCraft(typeof(HeadSlotChangeDeed), "Power Deeds", "HeadSlotChange Deed", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Fibrogen), "Fibrogen", 1, 1044253);
            AddRes(index, typeof(FlammablePlasmine), "FlammablePlasmine", 1, 1044253);
            index = AddCraft(typeof(LrcDeed), "Power Deeds", "LRC deed", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(FlammablePlasmine), "FlammablePlasmine", 1, 1044253);
            AddRes(index, typeof(Infinityclay), "Infinityclay", 1, 1044253);
            index = AddCraft(typeof(MurderRemovalDeed), "Power Deeds", "MurderRemoval Deed", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(TransparentAurarus), "TransparentAurarus", 1, 1044253);
            AddRes(index, typeof(Fibrogen), "Fibrogen", 1, 1044253);
            index = AddCraft(typeof(NeckSlotChangeDeed), "Power Deeds", "NeckSlotChange Deed", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(PositiveEvenium), "PositiveEvenium", 1, 1044253);
            AddRes(index, typeof(RefractivePotamite), "RefractivePotamite", 1, 1044253);
            index = AddCraft(typeof(OneHandedTransformDeed), "Power Deeds", "OneHandedTransform Deed", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(NegativePhocite), "NegativePhocite", 1, 1044253);
            AddRes(index, typeof(Infinityclay), "Infinityclay", 1, 1044253);
            index = AddCraft(typeof(PetBondDeed), "Power Deeds", "PetBond Deed", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(PositiveEvenium), "PositiveEvenium", 1, 1044253);
            AddRes(index, typeof(Infinityclay), "Infinityclay", 1, 1044253);
            index = AddCraft(typeof(PetSlotDeed), "Power Deeds", "PetSlot Deed", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Flurocite), "Flurocite", 1, 1044253);
            AddRes(index, typeof(Schizonite), "Schizonite", 1, 1044253);
            index = AddCraft(typeof(RingSlotChangeDeed), "Power Deeds", "RingSlotChange Deed", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Turbesium), "Turbesium", 1, 1044253);
            AddRes(index, typeof(ChargedAcoustesium), "ChargedAcoustesium", 1, 1044253);
            index = AddCraft(typeof(RoyalPetsCharter), "Power Deeds", "Royal Pets Charter", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(GraveNylon), "GraveNylon", 1, 1044253);
            AddRes(index, typeof(Uranimite), "Uranimite", 1, 1044253);
            index = AddCraft(typeof(RoyalSkillCharter), "Power Deeds", "Royal Skill Charter", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(DenaturedMorphonite), "DenaturedMorphonite", 1, 1044253);
            AddRes(index, typeof(Bloodglass), "Bloodglass", 1, 1044253);
            index = AddCraft(typeof(RoyalStatCharter), "Power Deeds", "Royal Stat Charter", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(MorphicHeteril), "MorphicHeteril", 1, 1044253);
            AddRes(index, typeof(Infinityclay), "Infinityclay", 1, 1044253);
            index = AddCraft(typeof(ShirtSlotChangeDeed), "Power Deeds", "ShirtSlotChange Deed", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(NegativePhocite), "NegativePhocite", 1, 1044253);
            AddRes(index, typeof(RefractivePotamite), "RefractivePotamite", 1, 1044253);
            index = AddCraft(typeof(SkillOrb), "Power Deeds", "Skill Orb", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Energite), "Energite", 1, 1044253);
            AddRes(index, typeof(Turbesium), "Turbesium", 1, 1044253);
            index = AddCraft(typeof(StatCapDeed), "Power Deeds", "Statcap deed", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Energite), "Energite", 1, 1044253);
            AddRes(index, typeof(Uranimite), "Uranimite", 1, 1044253);
            index = AddCraft(typeof(StatCapOrb), "Power Deeds", "StatCap Orb", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(ExoticEun), "ExoticEun", 1, 1044253);
            AddRes(index, typeof(Impervanium), "Impervanium", 1, 1044253);

			//Skill Codex
            index = AddCraft(typeof(AlchemySpellbook), "Skill Codex", "Alchemy Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(NegativePhocite), "NegativePhocite", 1, 1044253);
            AddRes(index, typeof(OpaqueHydragyon), "OpaqueHydragyon", 1, 1044253);
            AddRes(index, typeof(Fibrogen), "Fibrogen", 1, 1044253);
            index = AddCraft(typeof(FishingSpellbook), "Skill Codex", "Fishing Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(PositiveEvenium), "PositiveEvenium", 1, 1044253);
            AddRes(index, typeof(Turbesium), "Turbesium", 1, 1044253);
            AddRes(index, typeof(Infinityclay), "Infinityclay", 1, 1044253);
            index = AddCraft(typeof(EvalIntSpellbook), "Skill Codex", "EvalInt Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(Thoril), "Thoril", 1, 1044253);
            AddRes(index, typeof(PositiveEvenium), "PositiveEvenium", 1, 1044253);
            AddRes(index, typeof(HighdensityElectron), "HighdensityElectron", 1, 1044253);
            index = AddCraft(typeof(ArcherySpellbook), "Skill Codex", "Archery Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(Flurocite), "Flurocite", 1, 1044253);
            AddRes(index, typeof(NegativePhocite), "NegativePhocite", 1, 1044253);
            AddRes(index, typeof(Impervanium), "Impervanium", 1, 1044253);
            index = AddCraft(typeof(MagerySpellbook), "Skill Codex", "Magery Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(HighdensityElectron), "HighdensityElectron", 1, 1044253);
            AddRes(index, typeof(Thoril), "Thoril", 1, 1044253);
            AddRes(index, typeof(OpaqueHydragyon), "OpaqueHydragyon", 1, 1044253);
            index = AddCraft(typeof(ArmsLoreSpellbook), "Skill Codex", "ArmsLore Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(GaseousAesthogen), "GaseousAesthogen", 1, 1044253);
            AddRes(index, typeof(Turbesium), "Turbesium", 1, 1044253);
            AddRes(index, typeof(RefractivePotamite), "RefractivePotamite", 1, 1044253);
            index = AddCraft(typeof(AnimalTamingSpellbook), "Skill Codex", "AnimalTaming Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(Turbesium), "Turbesium", 1, 1044253);
            AddRes(index, typeof(NegativePhocite), "NegativePhocite", 1, 1044253);
            AddRes(index, typeof(ChargedAcoustesium), "ChargedAcoustesium", 1, 1044253);
            index = AddCraft(typeof(AnimalLoreSpellbook), "Skill Codex", "AnimalLore Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(OpaqueHydragyon), "OpaqueHydragyon", 1, 1044253);
            AddRes(index, typeof(RefractivePotamite), "RefractivePotamite", 1, 1044253);
            AddRes(index, typeof(Uranimite), "Uranimite", 1, 1044253);
            index = AddCraft(typeof(CarpentrySpellbook), "Skill Codex", "Carpentry Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(RefractivePotamite), "RefractivePotamite", 1, 1044253);
            AddRes(index, typeof(Negite), "Negite", 1, 1044253);
            AddRes(index, typeof(ChargedAcoustesium), "ChargedAcoustesium", 1, 1044253);
            index = AddCraft(typeof(CartographySpellbook), "Skill Codex", "Cartography Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(Flurocite), "Flurocite", 1, 1044253);
            AddRes(index, typeof(Impervanium), "Impervanium", 1, 1044253);
            AddRes(index, typeof(Schizonite), "Schizonite", 1, 1044253);
            index = AddCraft(typeof(TasteIDSpellbook), "Skill Codex", "TasteID Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(Impervanium), "Impervanium", 1, 1044253);
            AddRes(index, typeof(NegativePhocite), "NegativePhocite", 1, 1044253);
            AddRes(index, typeof(ChargedAcoustesium), "ChargedAcoustesium", 1, 1044253);
            index = AddCraft(typeof(CookingSpellbook), "Skill Codex", "Cooking Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(OpaqueHydragyon), "OpaqueHydragyon", 1, 1044253);
            AddRes(index, typeof(Fibrogen), "Fibrogen", 1, 1044253);
            AddRes(index, typeof(HighdensityElectron), "HighdensityElectron", 1, 1044253);
            index = AddCraft(typeof(DiscordanceSpellbook), "Skill Codex", "Discordance Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(Schizonite), "Schizonite", 1, 1044253);
            AddRes(index, typeof(Negite), "Negite", 1, 1044253);
            AddRes(index, typeof(Flurocite), "Flurocite", 1, 1044253);
            index = AddCraft(typeof(FencingSpellbook), "Skill Codex", "Fencing Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(Schizonite), "Schizonite", 1, 1044253);
            AddRes(index, typeof(NegativePhocite), "NegativePhocite", 1, 1044253);
            AddRes(index, typeof(MorphicHeteril), "MorphicHeteril", 1, 1044253);
            index = AddCraft(typeof(FletchingSpellbook), "Skill Codex", "Fletching Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(Flurocite), "Flurocite", 1, 1044253);
            AddRes(index, typeof(ChargedAcoustesium), "ChargedAcoustesium", 1, 1044253);
            AddRes(index, typeof(NegativePhocite), "NegativePhocite", 1, 1044253);
            index = AddCraft(typeof(ForensicsSpellbook), "Skill Codex", "Forensics Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(Uranimite), "Uranimite", 1, 1044253);
            AddRes(index, typeof(Bloodglass), "Bloodglass", 1, 1044253);
            AddRes(index, typeof(Energite), "Energite", 1, 1044253);
            index = AddCraft(typeof(WrestlingSpellbook), "Skill Codex", "Wrestling Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(Turbesium), "Turbesium", 1, 1044253);
            AddRes(index, typeof(Negite), "Negite", 1, 1044253);
            AddRes(index, typeof(FlammablePlasmine), "FlammablePlasmine", 1, 1044253);
            index = AddCraft(typeof(ParrySpellbook), "Skill Codex", "Parry Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(TransparentAurarus), "TransparentAurarus", 1, 1044253);
            AddRes(index, typeof(Fibrogen), "Fibrogen", 1, 1044253);
            AddRes(index, typeof(Impervanium), "Impervanium", 1, 1044253);
            index = AddCraft(typeof(HealingSpellbook), "Skill Codex", "Healing Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(Aeroglass), "Aeroglass", 1, 1044253);
            AddRes(index, typeof(Negite), "Negite", 1, 1044253);
            AddRes(index, typeof(NegativePhocite), "NegativePhocite", 1, 1044253);
            index = AddCraft(typeof(DetectHiddenSpellbook), "Skill Codex", "DetectHidden Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(FlammablePlasmine), "FlammablePlasmine", 1, 1044253);
            AddRes(index, typeof(NegativePhocite), "NegativePhocite", 1, 1044253);
            AddRes(index, typeof(HighdensityElectron), "HighdensityElectron", 1, 1044253);
            index = AddCraft(typeof(ProvocationSpellbook), "Skill Codex", "Provocation Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(Schizonite), "Schizonite", 1, 1044253);
            AddRes(index, typeof(NegativePhocite), "NegativePhocite", 1, 1044253);
            AddRes(index, typeof(RefractivePotamite), "RefractivePotamite", 1, 1044253);
            index = AddCraft(typeof(LockpickingSpellbook), "Skill Codex", "Lockpicking Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(Impervanium), "Impervanium", 1, 1044253);
            AddRes(index, typeof(GraveNylon), "GraveNylon", 1, 1044253);
            AddRes(index, typeof(RefractivePotamite), "RefractivePotamite", 1, 1044253);
            index = AddCraft(typeof(MacingSpellbook), "Skill Codex", "Macing Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(OpaqueHydragyon), "OpaqueHydragyon", 1, 1044253);
            AddRes(index, typeof(Aeroglass), "Aeroglass", 1, 1044253);
            AddRes(index, typeof(GaseousAesthogen), "GaseousAesthogen", 1, 1044253);
            index = AddCraft(typeof(MeditationSpellbook), "Skill Codex", "Meditation Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(Aeroglass), "Aeroglass", 1, 1044253);
            AddRes(index, typeof(ExoticEun), "ExoticEun", 1, 1044253);
            AddRes(index, typeof(Uranimite), "Uranimite", 1, 1044253);
            index = AddCraft(typeof(BeggingSpellbook), "Skill Codex", "Begging Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(Aeroglass), "Aeroglass", 1, 1044253);
            AddRes(index, typeof(ChargedAcoustesium), "ChargedAcoustesium", 1, 1044253);
            AddRes(index, typeof(Infinityclay), "Infinityclay", 1, 1044253);
            index = AddCraft(typeof(MiningSpellbook), "Skill Codex", "Mining Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(GaseousAesthogen), "GaseousAesthogen", 1, 1044253);
            AddRes(index, typeof(FlammablePlasmine), "FlammablePlasmine", 1, 1044253);
            AddRes(index, typeof(HighdensityElectron), "HighdensityElectron", 1, 1044253);
            index = AddCraft(typeof(ChivalrySpellbook2), "Skill Codex", "Chivalry Spellbook2", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(Schizonite), "Schizonite", 1, 1044253);
            AddRes(index, typeof(OpaqueHydragyon), "OpaqueHydragyon", 1, 1044253);
            AddRes(index, typeof(NegativePhocite), "NegativePhocite", 1, 1044253);
            index = AddCraft(typeof(StealingSpellbook), "Skill Codex", "Stealing Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(NegativePhocite), "NegativePhocite", 1, 1044253);
            AddRes(index, typeof(RefractivePotamite), "RefractivePotamite", 1, 1044253);
            AddRes(index, typeof(Flurocite), "Flurocite", 1, 1044253);
            index = AddCraft(typeof(InscribeSpellbook), "Skill Codex", "Inscribe Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(NegativePhocite), "NegativePhocite", 1, 1044253);
            AddRes(index, typeof(Impervanium), "Impervanium", 1, 1044253);
            AddRes(index, typeof(Fibrogen), "Fibrogen", 1, 1044253);
            index = AddCraft(typeof(NinjitsuSpellbook), "Skill Codex", "Ninjitsu Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(Impervanium), "Impervanium", 1, 1044253);
            AddRes(index, typeof(OpaqueHydragyon), "OpaqueHydragyon", 1, 1044253);
            AddRes(index, typeof(PositiveEvenium), "PositiveEvenium", 1, 1044253);
            index = AddCraft(typeof(HidingSpellbook), "Skill Codex", "Hiding Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(RefractivePotamite), "RefractivePotamite", 1, 1044253);
            AddRes(index, typeof(HighdensityElectron), "HighdensityElectron", 1, 1044253);
            AddRes(index, typeof(Aeroglass), "Aeroglass", 1, 1044253);
            index = AddCraft(typeof(StealthSpellbook), "Skill Codex", "Stealth Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(ChargedAcoustesium), "ChargedAcoustesium", 1, 1044253);
            AddRes(index, typeof(Aeroglass), "Aeroglass", 1, 1044253);
            AddRes(index, typeof(GraveNylon), "GraveNylon", 1, 1044253);
            index = AddCraft(typeof(BlacksmithSpellbook), "Skill Codex", "Blacksmith Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(PositiveEvenium), "PositiveEvenium", 1, 1044253);
            AddRes(index, typeof(DenaturedMorphonite), "DenaturedMorphonite", 1, 1044253);
            AddRes(index, typeof(Thoril), "Thoril", 1, 1044253);
            index = AddCraft(typeof(TacticsSpellbook), "Skill Codex", "Tactics Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(Impervanium), "Impervanium", 1, 1044253);
            AddRes(index, typeof(RefractivePotamite), "RefractivePotamite", 1, 1044253);
            AddRes(index, typeof(Energite), "Energite", 1, 1044253);
            index = AddCraft(typeof(SwordsSpellbook), "Skill Codex", "Swords Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(GraveNylon), "GraveNylon", 1, 1044253);
            AddRes(index, typeof(TransparentAurarus), "TransparentAurarus", 1, 1044253);
            AddRes(index, typeof(FlammablePlasmine), "FlammablePlasmine", 1, 1044253);
            index = AddCraft(typeof(TailoringSpellbook), "Skill Codex", "Tailoring Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(Schizonite), "Schizonite", 1, 1044253);
            AddRes(index, typeof(GraveNylon), "GraveNylon", 1, 1044253);
            AddRes(index, typeof(Flurocite), "Flurocite", 1, 1044253);
            index = AddCraft(typeof(NecromancySpellbook), "Skill Codex", "Necromancy Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(FlammablePlasmine), "FlammablePlasmine", 1, 1044253);
            AddRes(index, typeof(RefractivePotamite), "RefractivePotamite", 1, 1044253);
            AddRes(index, typeof(ChargedAcoustesium), "ChargedAcoustesium", 1, 1044253);
            index = AddCraft(typeof(TrackingSpellbook), "Skill Codex", "Tracking Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(Impervanium), "Impervanium", 1, 1044253);
            AddRes(index, typeof(FlammablePlasmine), "FlammablePlasmine", 1, 1044253);
            AddRes(index, typeof(Turbesium), "Turbesium", 1, 1044253);
            index = AddCraft(typeof(RemoveTrapSpellbook), "Skill Codex", "RemoveTrap Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(Impervanium), "Impervanium", 1, 1044253);
            AddRes(index, typeof(Turbesium), "Turbesium", 1, 1044253);
            AddRes(index, typeof(Flurocite), "Flurocite", 1, 1044253);
            index = AddCraft(typeof(VeterinarySpellbook), "Skill Codex", "Veterinary Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(DenaturedMorphonite), "DenaturedMorphonite", 1, 1044253);
            AddRes(index, typeof(MorphicHeteril), "MorphicHeteril", 1, 1044253);
            AddRes(index, typeof(RefractivePotamite), "RefractivePotamite", 1, 1044253);
            index = AddCraft(typeof(MusicianshipSpellbook), "Skill Codex", "Musicianship Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(PositiveEvenium), "PositiveEvenium", 1, 1044253);
            AddRes(index, typeof(MorphicHeteril), "MorphicHeteril", 1, 1044253);
            AddRes(index, typeof(GaseousAesthogen), "GaseousAesthogen", 1, 1044253);
            index = AddCraft(typeof(CampingSpellbook), "Skill Codex", "Camping Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(ExoticEun), "ExoticEun", 1, 1044253);
            AddRes(index, typeof(GraveNylon), "GraveNylon", 1, 1044253);
            AddRes(index, typeof(RefractivePotamite), "RefractivePotamite", 1, 1044253);
            index = AddCraft(typeof(LumberjackingSpellbook), "Skill Codex", "Lumberjacking Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(Uranimite), "Uranimite", 1, 1044253);
            AddRes(index, typeof(OpaqueHydragyon), "OpaqueHydragyon", 1, 1044253);
            AddRes(index, typeof(Impervanium), "Impervanium", 1, 1044253);
            index = AddCraft(typeof(SpiritSpeakSpellbook), "Skill Codex", "SpiritSpeak Spellbook", 70.0, 200.0, typeof(MagicalResidue), "MagicalResidue", 20, 1044037);
            AddRes(index, typeof(ExoticEun), "ExoticEun", 1, 1044253);
            AddRes(index, typeof(MorphicHeteril), "MorphicHeteril", 1, 1044253);
            AddRes(index, typeof(Flurocite), "Flurocite", 1, 1044253);			

			//Transcendence Scrolls
            index = AddCraft(typeof(ScrollOfTranscendenceAlchemy), "Transcendence Scrolls", "ScrollOfTranscendenceAlchemy", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(GaseousAesthogen), "GaseousAesthogen", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceAnatomy), "Transcendence Scrolls", "ScrollOfTranscendenceAnatomy", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(OpaqueHydragyon), "OpaqueHydragyon", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceAnimalLore), "Transcendence Scrolls", "ScrollOfTranscendenceAnimalLore", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(GraveNylon), "GraveNylon", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceAnimalTaming), "Transcendence Scrolls", "ScrollOfTranscendenceAnimalTaming", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(FlammablePlasmine), "FlammablePlasmine", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceArchery), "Transcendence Scrolls", "ScrollOfTranscendenceArchery", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Thoril), "Thoril", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceArmsLore), "Transcendence Scrolls", "ScrollOfTranscendenceArmsLore", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(GraveNylon), "GraveNylon", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceBegging), "Transcendence Scrolls", "ScrollOfTranscendenceBegging", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(OpaqueHydragyon), "OpaqueHydragyon", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceBlacksmith), "Transcendence Scrolls", "ScrollOfTranscendenceBlacksmith", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Energite), "Energite", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceBushido), "Transcendence Scrolls", "ScrollOfTranscendenceBushido", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(RefractivePotamite), "RefractivePotamite", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceCamping), "Transcendence Scrolls", "ScrollOfTranscendenceCamping", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Thoril), "Thoril", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceCarpentry), "Transcendence Scrolls", "ScrollOfTranscendenceCarpentry", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(ExoticEun), "ExoticEun", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceCartography), "Transcendence Scrolls", "ScrollOfTranscendenceCartography", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Turbesium), "Turbesium", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceChivalry), "Transcendence Scrolls", "ScrollOfTranscendenceChivalry", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(NegativePhocite), "NegativePhocite", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceCooking), "Transcendence Scrolls", "ScrollOfTranscendenceCooking", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Fibrogen), "Fibrogen", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceDetectHidden), "Transcendence Scrolls", "ScrollOfTranscendenceDetectHidden", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Turbesium), "Turbesium", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceDiscordance), "Transcendence Scrolls", "ScrollOfTranscendenceDiscordance", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Thoril), "Thoril", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceEvalInt), "Transcendence Scrolls", "ScrollOfTranscendenceEvalInt", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(GaseousAesthogen), "GaseousAesthogen", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceFencing), "Transcendence Scrolls", "ScrollOfTranscendenceFencing", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(TransparentAurarus), "TransparentAurarus", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceFishing), "Transcendence Scrolls", "ScrollOfTranscendenceFishing", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(HighdensityElectron), "HighdensityElectron", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceFletching), "Transcendence Scrolls", "ScrollOfTranscendenceFletching", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Bloodglass), "Bloodglass", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceFocus), "Transcendence Scrolls", "ScrollOfTranscendenceFocus", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Energite), "Energite", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceForensics), "Transcendence Scrolls", "ScrollOfTranscendenceForensics", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(GaseousAesthogen), "GaseousAesthogen", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceHealing), "Transcendence Scrolls", "ScrollOfTranscendenceHealing", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(NegativePhocite), "NegativePhocite", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceHerding), "Transcendence Scrolls", "ScrollOfTranscendenceHerding", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(GaseousAesthogen), "GaseousAesthogen", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceHiding), "Transcendence Scrolls", "ScrollOfTranscendenceHiding", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Fibrogen), "Fibrogen", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceImbuing), "Transcendence Scrolls", "ScrollOfTranscendenceImbuing", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(RefractivePotamite), "RefractivePotamite", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceInscription), "Transcendence Scrolls", "ScrollOfTranscendenceInscription", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(TransparentAurarus), "TransparentAurarus", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceItemID), "Transcendence Scrolls", "ScrollOfTranscendenceItemID", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(PositiveEvenium), "PositiveEvenium", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceLockpicking), "Transcendence Scrolls", "ScrollOfTranscendenceLockpicking", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(OpaqueHydragyon), "OpaqueHydragyon", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceLumberjacking), "Transcendence Scrolls", "ScrollOfTranscendenceLumberjacking", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(ExoticEun), "ExoticEun", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceMaceFighting), "Transcendence Scrolls", "ScrollOfTranscendenceMaceFighting", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Energite), "Energite", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceMagery), "Transcendence Scrolls", "ScrollOfTranscendenceMagery", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Flurocite), "Flurocite", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceMagicResist), "Transcendence Scrolls", "ScrollOfTranscendenceMagicResist", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Negite), "Negite", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceMeditation), "Transcendence Scrolls", "ScrollOfTranscendenceMeditation", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(RefractivePotamite), "RefractivePotamite", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceMining), "Transcendence Scrolls", "ScrollOfTranscendenceMining", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(NegativePhocite), "NegativePhocite", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceMusicianship), "Transcendence Scrolls", "ScrollOfTranscendenceMusicianship", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(HighdensityElectron), "HighdensityElectron", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceMysticism), "Transcendence Scrolls", "ScrollOfTranscendenceMysticism", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(DenaturedMorphonite), "DenaturedMorphonite", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceNecromancy), "Transcendence Scrolls", "ScrollOfTranscendenceNecromancy", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Bloodglass), "Bloodglass", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceNinjitsu), "Transcendence Scrolls", "ScrollOfTranscendenceNinjitsu", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(FlammablePlasmine), "FlammablePlasmine", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceParry), "Transcendence Scrolls", "ScrollOfTranscendenceParry", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(NegativePhocite), "NegativePhocite", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendencePeacemaking), "Transcendence Scrolls", "ScrollOfTranscendencePeacemaking", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(GraveNylon), "GraveNylon", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendencePoisoning), "Transcendence Scrolls", "ScrollOfTranscendencePoisoning", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(HighdensityElectron), "HighdensityElectron", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceProvocation), "Transcendence Scrolls", "ScrollOfTranscendenceProvocation", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Uranimite), "Uranimite", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceRemoveTrap), "Transcendence Scrolls", "ScrollOfTranscendenceRemoveTrap", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Uranimite), "Uranimite", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceSnooping), "Transcendence Scrolls", "ScrollOfTranscendenceSnooping", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Impervanium), "Impervanium", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceSpellweaving), "Transcendence Scrolls", "ScrollOfTranscendenceSpellweaving", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Uranimite), "Uranimite", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceSpiritSpeak), "Transcendence Scrolls", "ScrollOfTranscendenceSpiritSpeak", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Thoril), "Thoril", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceStealing), "Transcendence Scrolls", "ScrollOfTranscendenceStealing", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(FlammablePlasmine), "FlammablePlasmine", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceStealth), "Transcendence Scrolls", "ScrollOfTranscendenceStealth", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(ChargedAcoustesium), "ChargedAcoustesium", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceSwordsmanship), "Transcendence Scrolls", "ScrollOfTranscendenceSwordsmanship", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(NegativePhocite), "NegativePhocite", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceTactics), "Transcendence Scrolls", "ScrollOfTranscendenceTactics", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Impervanium), "Impervanium", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceTailoring), "Transcendence Scrolls", "ScrollOfTranscendenceTailoring", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(MorphicHeteril), "MorphicHeteril", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceTasteID), "Transcendence Scrolls", "ScrollOfTranscendenceTasteID", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Uranimite), "Uranimite", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceThrowing), "Transcendence Scrolls", "ScrollOfTranscendenceThrowing", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(ExoticEun), "ExoticEun", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceTinkering), "Transcendence Scrolls", "ScrollOfTranscendenceTinkering", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(NegativePhocite), "NegativePhocite", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceTracking), "Transcendence Scrolls", "ScrollOfTranscendenceTracking", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Negite), "Negite", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceVeterinary), "Transcendence Scrolls", "ScrollOfTranscendenceVeterinary", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Aeroglass), "Aeroglass", 1, 1044253);
            index = AddCraft(typeof(ScrollOfTranscendenceWrestling), "Transcendence Scrolls", "ScrollOfTranscendenceWrestling", 30.0, 200.0, typeof(BlankScroll), "BlankScroll", 1, 1044037);
            AddRes(index, typeof(Aeroglass), "Aeroglass", 1, 1044253);

            MarkOption = true;
        }
    }
}
