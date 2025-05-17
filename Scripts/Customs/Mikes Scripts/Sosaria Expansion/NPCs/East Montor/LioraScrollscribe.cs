using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class HeirsDownfallQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Heir’s Downfall"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Liora Scrollscribe*, the vigilant Historian of East Montor, cloaked in deep violet and dusk-grey, her eyes reflecting the weight of countless chronicles.\n\n" +
                    "“Have you seen the crypt’s frescoes?” she begins, voice low and urgent. “They depict a line of kings, but one—the *DrakkonHeir*—is marred. Forgotten, erased... yet now, **he walks again**, desecrating the ancient crypt with his presence.”\n\n" +
                    "“The heir was cast out long ago, said to have consorted with dragons, twisted by greed and flame. Now he rises, corrupting our sacred past.”\n\n" +
                    "“You must stop him. If he is not felled, our records, our truth, may unravel. **The heir must fall.**”\n\n" +
                    "**Slay the DrakkonHeir** haunting the Caves of Drakkon.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the crypt remain defiled, and the truth of East Montor’s lineage forever obscured.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The DrakkonHeir still haunts us? His shadow lengthens. Our past trembles on the brink of oblivion.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done what history demanded.\n\n" +
                       "**The heir is no more.** His blight is lifted, and the crypt breathes again.\n\n" +
                       "Take this—*FleshLight*. It is both a beacon and a blade, to guard you when memory falters and shadows gather.";
            }
        }

        public HeirsDownfallQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DrakkonHeir), "DrakkonHeir", 1));
            AddReward(new BaseReward(typeof(FleshLight), 1, "FleshLight"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Heir’s Downfall'!");
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

    public class LioraScrollscribe : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(HeirsDownfallQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBScribe(this));
        }

        [Constructable]
        public LioraScrollscribe()
            : base("the Town Historian", "Liora Scrollscribe")
        {
        }

        public LioraScrollscribe(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 80, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1150; // Pale
            HairItemID = 0x2044; // Long Hair
            HairHue = 1153; // Silver-white
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1157, Name = "Historian's Robe of Twilight" }); // Deep violet
            AddItem(new Cloak() { Hue = 1109, Name = "Duskveil Cloak" }); // Dust-gray
            AddItem(new Boots() { Hue = 1108, Name = "Archivist’s Boots" }); // Dark
            AddItem(new BodySash() { Hue = 1153, Name = "Sash of Forgotten Tales" }); // Silver-white
            AddItem(new WizardsHat() { Hue = 1157, Name = "Scrollscribe's Hat" }); // Matching deep violet

            AddItem(new ScribeSword() { Hue = 1109, Name = "Scriptor’s Blade" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1175; // Slightly iridescent blue
            backpack.Name = "Scrollbinder’s Pack";
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
