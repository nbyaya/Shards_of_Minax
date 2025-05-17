using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SubdueInfernoDevilQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Subdue the InfernoDevil"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Aurelia Flameborn*, Guild Leader of West Montor’s Merchant Circle.\n\n" +
                    "Her eyes burn with intensity, hair braided with crimson cords, armor glowing faintly as if kissed by firelight.\n\n" +
                    "\"We had a deal... once. I bartered with the lesser fiends, kept the fires at bay in exchange for tribute. But this new one—**InfernoDevil**—it shattered our pact. Melted the coffers, threatened our halls with molten ruin.\"\n\n" +
                    "\"The guild won’t bend again. We need that thing banished, or our trade will burn to ash.\"\n\n" +
                    "**Slay the InfernoDevil** in the Gate of Hell, and restore balance to our city’s flame-forged resolve.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "\"Then the fires will rise unchecked. I hope you’ve a place far from West Montor’s embers.\"";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "\"Still it rages? The molten gold has begun to seep into the streets. Act quickly, before the guildhall crumbles under its own weight.\"";
            }
        }

        public override object Complete
        {
            get
            {
                return "\"It is done. The InfernoDevil’s roar is silenced, and with it, our debts to the fire.\n\n" +
                       "Take this: **EnderGuardiansChestplate**—forged in the flames we once feared, now a symbol of our defiance.\"";
            }
        }

        public SubdueInfernoDevilQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(InfernoDevil), "InfernoDevil", 1));
            AddReward(new BaseReward(typeof(EnderGuardiansChestplate), 1, "EnderGuardiansChestplate"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Subdue the InfernoDevil'!");
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

    public class AureliaFlameborn : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SubdueInfernoDevilQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHairStylist());
        }

        [Constructable]
        public AureliaFlameborn()
            : base("the Flameborn Guild Leader", "Aurelia Flameborn")
        {
        }

        public AureliaFlameborn(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 100, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x2049; // Braided hair
            HairHue = 1359; // Fiery red
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest() { Hue = 2075, Name = "Molten-Heart Cuirass" }); // Fiery red-orange
            AddItem(new PlateArms() { Hue = 2075, Name = "Inferno-Tempered Vambraces" });
            AddItem(new StuddedGloves() { Hue = 2407, Name = "Ashen Grasp" });
            AddItem(new PlateLegs() { Hue = 2075, Name = "Ember-Wrought Greaves" });
            AddItem(new BodySash() { Hue = 1157, Name = "Guild Flame-Sash" }); // Deep red
            AddItem(new Cloak() { Hue = 1153, Name = "Flarecloak of the Pact" }); // Crimson
            AddItem(new PlateHelm() { Hue = 2075, Name = "Blazeguard Helm" });

            AddItem(new WarHammer() { Hue = 2075, Name = "Inferno Pact-Hammer" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Guild Ledger Pack";
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
