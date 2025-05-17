using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ArchivistsFallQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Archivist's Fall"; } }

        public override object Description
        {
            get
            {
                return
                    "You find yourself before *Isolde Scrollcaller*, the solemn Head Archivist of Castle British.\n\n" +
                    "Her fingers trace the spine of a sealed tome, eyes distant yet resolute.\n\n" +
                    "“TheArchivist was once my mentor… and friend. Together, we charted the depths of Sosaria’s history, guarding it against decay.”\n\n" +
                    "“But the Vault changed him. He sought to ‘perfect’ history, to rewrite flaws, erase pain. I warned him, but his mind was already lost to the echoes of the old world.”\n\n" +
                    "“Now he lingers in **Preservation Vault 44**, twisting our past, threatening to unravel truth itself.”\n\n" +
                    "“Slay him. **Slay TheArchivist**. If he completes his work, all we are—**all we ever were**—will be no more than a lie.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the truth guard itself… though I fear it shall not.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "He still writes? Then history frays at the seams… we are running out of time.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It is done? Then the quills still. Thank you, though the ink of betrayal never truly fades.\n\n" +
                       "Take this: *CollarOfTheGildedSecret*. Woven with the knowledge I could not save, may it guard you from ever being so blind.";
            }
        }

        public ArchivistsFallQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(TheArchivist), "TheArchivist", 1));
            AddReward(new BaseReward(typeof(CollarOfTheGildedSecret), 1, "CollarOfTheGildedSecret"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Archivist's Fall'!");
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

    public class IsoldeScrollcaller : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ArchivistsFallQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBScribe(this));
        }

        [Constructable]
        public IsoldeScrollcaller()
            : base("the Head Archivist", "Isolde Scrollcaller")
        {
        }

        public IsoldeScrollcaller(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 70, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1153; // Pale, mystical hue
            HairItemID = 0x2047; // Long hair
            HairHue = 1150; // Silver-white
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1157, Name = "Archivist's Gown of Lost Memories" }); // Midnight blue
            AddItem(new Cloak() { Hue = 1151, Name = "Cloak of Silent Pages" }); // Light silver
            AddItem(new Sandals() { Hue = 1109, Name = "Silent Step Sandals" }); // Dusty grey
            AddItem(new SkullCap() { Hue = 1150, Name = "Scholar's Cap of Forgotten Truths" }); // Pale silver

            AddItem(new SpellWeaversWand() { Hue = 1153, Name = "Sealed Quill of Unwritten Lore" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1154;
            backpack.Name = "Scrollkeeper's Pack";
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
