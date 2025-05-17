using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;


namespace Server.Engines.Quests
{
    public class ChitteringScourgeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Chittering Scourge"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Tiberius Firebane*, the scarred alchemist of Yew, whose hands tremble not from age, but memory.\n\n" +
                    "Clad in flame-hued robes, he leans over a glowing cauldron, his eyes reflecting the flickering light of volatile concoctions.\n\n" +
                    "\"PlagueImps... vile creatures of pestilence. One nearly ended me, years ago, on a caravan trail through Catastrophe. I bear its mark still.\"\n\n" +
                    "\"Yet from their claws, a cure can be born. Their essence, when distilled, wards against the rot that now stalks our woods. I need its talons—**fresh**, not bartered from some smuggler’s pouch.\"\n\n" +
                    "**Slay the PlagueImp** in Catastrophe, and bring me its claws. For the health of Yew, and the silence of my past.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the forest suffer its creeping death. But know this: without the cure, even you may feel the imp’s sting before long.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "You have yet to face the imp? Each day it breeds more corruption beneath Catastrophe’s shroud.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The claws... still slick with venomous spite. Perfect.\n\n" +
                       "With these, the cure shall burn through the sickness as fire through dry rot.\n\n" +
                       "Take this: *ThroatOfTheRoaringFlame*. May its power grant you voice enough to command even the plague to heel.";
            }
        }

        public ChitteringScourgeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(PlagueImp), "PlagueImp", 1));
            AddReward(new BaseReward(typeof(ThroatOfTheRoaringFlame), 1, "ThroatOfTheRoaringFlame"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Chittering Scourge'!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class TiberiusFirebane : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ChitteringScourgeQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBAlchemist( this ) );
		}


        [Constructable]
        public TiberiusFirebane()
            : base("the Scorched Alchemist", "Tiberius Firebane")
        {
        }

        public TiberiusFirebane(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 90, 100);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1050; // Ashen complexion
            HairItemID = 0x2048; // Short hair
            HairHue = 1154; // Fiery red-orange
            FacialHairItemID = 0x2041; // Goatee
            FacialHairHue = 1154;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1359, Name = "Ashenfire Robe" }); // Smoky crimson
            AddItem(new HoodedShroudOfShadows() { Hue = 1175, Name = "Ember Hood" }); // Deep ember
            AddItem(new LeatherGloves() { Hue = 1193, Name = "Charred Alchemist's Gloves" }); // Burnt leather
            AddItem(new Sandals() { Hue = 1157, Name = "Soot-Stained Sandals" }); // Charcoal black
            AddItem(new HalfApron() { Hue = 1161, Name = "Crimson Flask Apron" }); // Deep red

            Backpack backpack = new Backpack();
            backpack.Hue = 1175;
            backpack.Name = "Firebane's Satchel";
            AddItem(backpack);

            AddItem(new SpellWeaversWand() { Hue = 1359, Name = "Flamebinder Wand" });
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
