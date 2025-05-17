using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ToothOfTormentQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Tooth of Torment"; } }

        public override object Description
        {
            get
            {
                return
                    "The moonlight speaks in riddles, and I have heard its latest cry...\n\n" +
                    "A nightmare prowls our streets, unseen yet felt—a gaping maw known as the CursedMolar, " +
                    "gnawing at the dreams of Moon’s children, leaving them hollow, restless.\n\n" +
                    "**Seek out and destroy the Cursed Molar**, before its hunger grows too great, and our minds are consumed.";
            }
        }

        public override object Refuse { get { return "Then pray your dreams remain untouched by its bite."; } }

        public override object Uncomplete { get { return "The gnawing persists. You have not yet silenced the cursed tooth."; } }

        public override object Complete { get { return "The dreams quiet... for now. Take Whisperfang, and guard your soul."; } }

        public ToothOfTormentQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CursedMolar), "CursedMolars", 1));
            AddReward(new BaseReward(typeof(Whisperfang), 1, "Whisperfang"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Tooth of Torment'!");
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

    public class ElaraMirrorwind : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ToothOfTormentQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMage()); 
        }

        [Constructable]
        public ElaraMirrorwind() : base("Elara Mirrorwind", "Seer of Moon")
        {
            Title = "Seer of Moon";
			Body = 0x191; // Male human
			Female = true;

            // Outfit
            AddItem(new HoodedShroudOfShadows { Hue = 1150, Name = "Veil of the Pale Dream" }); // A shimmering silver-blue hooded robe
            AddItem(new Cloak { Hue = 2105, Name = "Mantle of Whispering Stars" }); // Deep indigo cloak, embroidered with constellations
            AddItem(new Sandals { Hue = 2101, Name = "Silent Step Sandals" }); // Soft midnight sandals that make no sound
            AddItem(new BodySash { Hue = 1153, Name = "Moonwoven Sash" }); // Sash that seems to glow faintly under moonlight
            AddItem(new WizardsHat { Hue = 1170, Name = "Crown of Lunar Insight" }); // Elegant, soft silver-blue hue, trimmed with pale gems

            // Gear for flair
            AddItem(new SpellWeaversWand { Hue = 2401, Name = "Mirrorwind's Focus" }); // Wand with a crystal tip reflecting light oddly
            AddItem(new Backpack { Hue = 2116, Name = "Seer's Satchel" }); // Carries dream-scrolls and stargazing tools

            SetStr(50, 60);
            SetDex(60, 70);
            SetInt(90, 100);

            SetDamage(3, 6);
            SetHits(150, 170);
        }

        public ElaraMirrorwind(Serial serial) : base(serial) { }

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
