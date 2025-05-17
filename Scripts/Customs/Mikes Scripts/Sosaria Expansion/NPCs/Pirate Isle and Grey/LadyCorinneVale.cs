using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WarlordsReckoningQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Warlord’s Reckoning"; } }

        public override object Description
        {
            get
            {
                return
                    "You find yourself face-to-face with **Lady Corinne Vale**, the noble patron of Grey’s coastal galleries. Her poise is as sharp as her gaze, her voice steady, hiding a flicker of old pain.\n\n" +
                    "“You’ve heard of the **Cadaverous Warlord**, no doubt. A butcher in life, a monster in death. Centuries ago, his war bands struck at Grey’s very heart, nearly toppling our eastern palisade. It was my ancestors who stood firm, wielding not just steel, but the will to resist.”\n\n" +
                    "“Now his specter haunts **Exodus Dungeon**, clad in the same bone-forged helm that grinned through our darkest nights. His war games are not over, and I will not let his legacy grow unchecked.”\n\n" +
                    "**Slay the Cadaverous Warlord** and bring me his helm. Let Grey know peace, and let my family’s honor be sealed in your victory.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then Grey shall wait for another to honor its defenders. But beware, for the Warlord’s reach is long, and his thirst unquenched.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "His helm eludes you still? Every moment he draws breath—if such creatures can breathe—our walls remember the old fear.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The helm… it is real. He is no more.\n\n" +
                       "**Grey stands unbroken, as do I.**\n\n" +
                       "Take this, *the Archer’s Yew Bow*. May it serve you as you’ve served my people. Our debt is great, and our thanks greater still.";
            }
        }

        public WarlordsReckoningQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CadaverousWarlord), "Cadaverous Warlord", 1));
            AddReward(new BaseReward(typeof(ArchersYewBow), 1, "Archer’s Yew Bow"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Warlord’s Reckoning'!");
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

    public class LadyCorinneVale : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(WarlordsReckoningQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBarkeeper());
        }

        [Constructable]
        public LadyCorinneVale()
            : base("the Noble Patron of the Arts", "Lady Corinne Vale")
        {
        }

        public LadyCorinneVale(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 90, 70);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1052; // Elegant pale
            HairItemID = 0x203C; // Long hair
            HairHue = 1150; // Silver-blonde
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 2117, Name = "Twilight Silk Gown" }); // Deep violet
            AddItem(new BodySash() { Hue = 1153, Name = "Vale Family Sash" }); // Cerulean
            AddItem(new Cloak() { Hue = 2105, Name = "Shadowed Cloak of Grey" }); // Dark charcoal
            AddItem(new FeatheredHat() { Hue = 1150, Name = "Noblewoman’s Plume" }); // Pale silver
            AddItem(new Sandals() { Hue = 2401, Name = "Ornate Sandals" }); // Midnight blue

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Silken Satchel";
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
