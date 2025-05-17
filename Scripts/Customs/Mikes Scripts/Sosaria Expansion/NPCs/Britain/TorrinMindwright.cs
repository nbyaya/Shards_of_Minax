using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class MindsEndQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Mind’s End"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Torrin Mindwright*, a once-proud scholar of Castle British’s arcane archives.\n\n" +
                    "His robe is frayed, eyes shadowed with sleepless nights, yet glowing faintly with desperate clarity.\n\n" +
                    "“There is... something loose in Preservation Vault 44,” he murmurs, voice tight with fear.\n\n" +
                    "“I entered seeking knowledge, but awoke with my mind... eroded. My name was gone, my thoughts jumbled, until I carved these runes.”\n\n" +
                    "He shows you faintly glowing glyphs scrawled across his wrists.\n\n" +
                    "“The *MemoryRot* festers there, consuming the memories of the vault's staff, guests... and me. I barely escaped. If left unchecked, it will unravel what remains of the Imperium’s truths.”\n\n" +
                    "“You must go. Follow the runes I inscribed. Anchor your mind—and **slay the MemoryRot**. Before it becomes all we remember.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then be careful where you tread. The Rot lingers, and it may find your mind just as... fragile.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "You have not yet slain it? I feel its tendrils even now, grasping at my memories. There is little time left.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it... the whispers are fading. My mind is clearer now, though not whole.\n\n" +
                       "Take this: *The Grove’s Final Reach*. It was a gift from a friend—before I forgot her name. May it remind you that some memories are worth the fight.";
            }
        }

        public MindsEndQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MemoryRot), "MemoryRot", 1));
            AddReward(new BaseReward(typeof(TheGrovesFinalReach), 1, "The Grove’s Final Reach"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Mind’s End'!");
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

    public class TorrinMindwright : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(MindsEndQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMage());
        }

        [Constructable]
        public TorrinMindwright()
            : base("the Arcane Scholar", "Torrin Mindwright")
        {
        }

        public TorrinMindwright(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 65, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1005; // Pale, slightly worn

            HairItemID = 0x203B; // Long hair
            HairHue = 1153; // Faded blue-gray
            FacialHairItemID = 0x204B; // Goatee
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1150, Name = "Mnemonic Robe" }); // A shimmering silver-blue
            AddItem(new WizardsHat() { Hue = 1109, Name = "Mindward Hat" }); // Deep gray
            AddItem(new Sandals() { Hue = 1157, Name = "Anchor Sandals" }); // Soft violet
            AddItem(new BodySash() { Hue = 1151, Name = "Rune-Scribed Sash" }); // Pale cyan, etched with faint runes
            AddItem(new SpellWeaversWand() { Hue = 1154, Name = "Runewood Wand" }); // Dark indigo wand

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Rune-Keeper's Pack";
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
