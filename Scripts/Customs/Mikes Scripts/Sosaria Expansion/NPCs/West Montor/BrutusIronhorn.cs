using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class InfernalMinotaurQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Shatter the Infernal Minotaur"; } }

        public override object Description
        {
            get
            {
                return
                    "Brutus Ironhorn, the famed *Arena Champion*, eyes you with grim determination.\n\n" +
                    "His armor is scarred, yet gleams with pride. The horned helm upon his brow is of ancient design, burnished with victories past.\n\n" +
                    "“I’ve faced beasts, monsters, and demons... but this one—this *Infernal Minotaur*—it threatens the ring, my legacy. It crawled from the depths of the Gate of Hell, carrying fire in its eyes and death in its roar.”\n\n" +
                    "“I’ve fought it once. Barely escaped. I need someone... no, I need a **warrior** to finish what I started. Slay this beast, and you'll not only save my arena—you’ll claim the *DemonspikeGuard*, forged for champions, not cowards.”\n\n" +
                    "**Kill the Infernal Minotaur** before it reduces West Montor’s pride to ashes.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then leave the ring to rot, and let the beast grow fat on my shame.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it lives? My honor burns with every breath that monster draws.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You did it? The beast lies dead? Hah!\n\n" +
                       "You’ve the strength of ten men, and the heart of a true champion. The ring stands, thanks to you.\n\n" +
                       "**Take this—DemonspikeGuard.** May it serve you in every battle yet to come.";
            }
        }

        public InfernalMinotaurQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(InfernalMinotaur), "Infernal Minotaur", 1));
            AddReward(new BaseReward(typeof(DemonspikeGuard), 1, "DemonspikeGuard"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Shatter the Infernal Minotaur'!");
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

    public class BrutusIronhorn : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(InfernalMinotaurQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBSwordWeapon()); // He is an Arena Champion; sword trainer/vendor
        }

        [Constructable]
        public BrutusIronhorn()
            : base("the Arena Champion", "Brutus Ironhorn")
        {
        }

        public BrutusIronhorn(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 90, 85);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 33770; // Rugged bronze skin
            HairItemID = 8251; // Warrior's long hair
            HairHue = 1109; // Charcoal black
            FacialHairItemID = 8267; // Thick beard
            FacialHairHue = 1109;
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest() { Hue = 2425, Name = "Champion's Warplate" }); // Dark iron
            AddItem(new PlateLegs() { Hue = 2425, Name = "Inferno Greaves" });
            AddItem(new PlateArms() { Hue = 2425, Name = "Brutal Armguards" });
            AddItem(new PlateGloves() { Hue = 2425, Name = "Gauntlets of the Ring" });
            AddItem(new HornedTribalMask() { Hue = 2117, Name = "Ironhorn's Helm" }); // Deep crimson, horned helm
            AddItem(new Cloak() { Hue = 2075, Name = "Mantle of Victory" }); // Blood-red cloak
            AddItem(new BodySash() { Hue = 1358, Name = "Champion's Sash" }); // Gold-trimmed

            AddItem(new Broadsword() { Hue = 2425, Name = "Arena Edge" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1175;
            backpack.Name = "Champion’s Pack";
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
