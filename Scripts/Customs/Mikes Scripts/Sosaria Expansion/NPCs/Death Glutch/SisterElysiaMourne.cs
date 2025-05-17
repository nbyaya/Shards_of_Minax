using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class OutbreakContainedQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Outbreak Contained"; } }

        public override object Description
        {
            get
            {
                return
                    "Sister *Elysia Mourne*, Plague Healer of Death Glutch, stands amidst rows of sickbeds, her robes heavy with the scent of herbal smoke.\n\n" +
                    "Her eyes, sharp and tired, meet yours with a glimmer of hope—and dread.\n\n" +
                    "“We’ve fought fevers, coughs, and even shadow-plagues... but this? This is no illness of flesh. It’s a thing—a *MacroVirus*, feeding in the depths of Malidor Witches Academy.”\n\n" +
                    "“I’ve lost patients—friends. My journals are filled with their last words. If it spreads beyond the Academy, Death Glutch won’t survive.”\n\n" +
                    "**Destroy the MacroVirus** that haunts the infirmary beneath the Academy. Bring peace to the fallen... and to those still clinging to life.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then pray you do not breathe in what they did. I will fight it alone, if I must.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "You’ve not yet slain it? The sickness spreads—I feel it in my blood, in their cries. Hurry!";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it... the air feels lighter, the whispers—gone. You’ve spared us an unspeakable fate.\n\n" +
                       "Take this *MarbleHourglass*—it’s all I can offer, but it’s precious to me. It once belonged to a healer who believed time was the truest cure.\n\n" +
                       "**May it guard your moments as you’ve guarded ours.**";
            }
        }

        public OutbreakContainedQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MacroVirus), "MacroVirus", 1));
            AddReward(new BaseReward(typeof(MarbleHourglass), 1, "MarbleHourglass"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Outbreak Contained'!");
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

    public class SisterElysiaMourne : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(OutbreakContainedQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHealer());
        }

        [Constructable]
        public SisterElysiaMourne()
            : base("the Plague Healer", "Sister Elysia Mourne")
        {
        }

        public SisterElysiaMourne(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 90, 30);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1153; // Pale skin tone
            HairItemID = 0x203C; // Long hair
            HairHue = 1102; // Moonlit silver
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 2418, Name = "Veil of Mourning" }); // Deep Plague Violet
            AddItem(new BodySash() { Hue = 2101, Name = "Sisterhood Sash" }); // Pale lavender
            AddItem(new Sandals() { Hue = 1109, Name = "Ashwalkers" }); // Dust gray
            AddItem(new HoodedShroudOfShadows() { Hue = 2101, Name = "Healer’s Hood" }); // Soft lavender hue

            Backpack backpack = new Backpack();
            backpack.Hue = 1150; // Quarantine gray
            backpack.Name = "Quarantine Satchel";
            AddItem(backpack);

            AddItem(new MagicWand() { Hue = 1157, Name = "Cleansing Rod" }); // Ethereal blue
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
