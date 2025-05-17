using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ExtinguishTheBlightQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Extinguish the Blight"; } }

        public override object Description
        {
            get
            {
                return
                    "Baelor Ironhand stands firm, the air thick with tension.\n\n" +
                    "He clenches a worn helm under his arm, its edges scorched. His other hand grips the pommel of his sword with grim determination.\n\n" +
                    "\"I've seen too much fire in my years, stranger. And now, the flames come for **Yew**.\"\n\n" +
                    "**\"The BlightedEfreet stirs in the depths of Catastrophe. Its fiery breath has already blackened sections of our walls—my men say the soil itself smolders. We can't let it spread. Not again.\"**\n\n" +
                    "\"My father fought during the Crimson Uprising. Faced down a fire demon much like this one. He barely survived. I won’t gamble our town’s fate.\"\n\n" +
                    "**\"Slay the BlightedEfreet** before its curse roots itself in our land. Bring me proof, and you'll have more than my thanks—you'll have earned the Crescent Roar.\"";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then I pray Yew's walls hold... though I've seen what happens when fire is left unchecked.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The fires rise higher. The land burns beneath us. Have you faced the Efreet?";
            }
        }

        public override object Complete
        {
            get
            {
                return "The flames have died... for now.\n\n" +
                       "You've done what I feared none could do. You didn't just save Yew, you've spared us from becoming a memory, scorched into the soil.\n\n" +
                       "**Take this—*SkirtOfTheCrescentRoar*. May its strength echo in your steps, as your deeds now echo in our hearts.**";
            }
        }

        public ExtinguishTheBlightQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BlightedEfreet), "BlightedEfreet", 1));
            AddReward(new BaseReward(typeof(SkirtOfTheCrescentRoar), 1, "SkirtOfTheCrescentRoar"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Extinguish the Blight'!");
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

    public class BaelorIronhand : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ExtinguishTheBlightQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBlacksmith()); 
        }

        [Constructable]
        public BaelorIronhand()
            : base("the Town Guard Captain", "Baelor Ironhand")
        {
        }

        public BaelorIronhand(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 80, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1023; // Weathered, tanned skin tone
            HairItemID = 0x2048; // Long hair
            HairHue = 1150; // Charcoal black
            FacialHairItemID = 0x2041; // Long beard
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest() { Hue = 1175, Name = "Ironhand's Breastplate" }); // Dull grey-blue steel
            AddItem(new PlateLegs() { Hue = 1175, Name = "Ironshod Greaves" });
            AddItem(new StuddedGloves() { Hue = 1109, Name = "Sooted Gauntlets" }); // Burn-marked leather
            AddItem(new PlateGorget() { Hue = 1175, Name = "Vigilant Gorget" });
            AddItem(new Cloak() { Hue = 2213, Name = "Crimson Watch Cloak" }); // Deep red
            AddItem(new Boots() { Hue = 1102, Name = "Ashen Warboots" });

            AddItem(new VikingSword() { Hue = 2401, Name = "Blazeguard" }); // Steel blue blade with fire-resistant lore
            AddItem(new BashingShield() { Hue = 1175, Name = "Wall-Keeper's Bulwark" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Captain's Field Pack";
            AddItem(backpack);
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
