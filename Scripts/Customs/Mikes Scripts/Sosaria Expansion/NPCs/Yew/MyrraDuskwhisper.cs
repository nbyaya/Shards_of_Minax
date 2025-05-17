using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class MirrorsEdgeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Mirror’s Edge"; } }

        public override object Description
        {
            get
            {
                return
                    "You find yourself in the presence of *Myrra Duskwhisper*, the Mirror Mage of Yew.\n\n" +
                    "She gazes into a hand-held shard of a shattered mirror, her voice barely above a whisper but sharp with urgency.\n\n" +
                    "“The town is not what it seems. Reflections lie. Faces… lie. Something has come through the glass. **An Eerie Doppelganger**.”\n\n" +
                    "“My spells, my mirrors—once used to glimpse hidden truths—have been corrupted. There are broken mirrors scattered across Yew, all pointing to a lair beneath, where **it feeds on stolen essences**.”\n\n" +
                    "“I need someone not bound to the reflection. Someone real. **Slay the Eerie Doppelganger**, free those it has mimicked, and bring balance back to Yew’s soul.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "“Then beware the next face you trust. It may not be your own.”";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "“You’ve not shattered the imposter yet? Every moment you wait, more of Yew’s soul fades into glass.”";
            }
        }

        public override object Complete
        {
            get
            {
                return "“It’s done. I can feel the mirrors calm. You’ve freed them… freed us.”\n\n" +
                       "“Take this: the *Crown of the Forgotten Oath*. It is bound to the real, forged from truth itself. May it shield you from illusion.”";
            }
        }

        public MirrorsEdgeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(EerieDoppleganger), "Eerie Doppelganger", 1));
            AddReward(new BaseReward(typeof(CrownOfTheForgottenOath), 1, "Crown of the Forgotten Oath"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Mirror’s Edge'!");
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

    public class MyrraDuskwhisper : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(MirrorsEdgeQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMage());
        }

        [Constructable]
        public MyrraDuskwhisper()
            : base("the Mirror Mage", "Myrra Duskwhisper")
        {
        }

        public MyrraDuskwhisper(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 75, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1153; // Pale mystical hue
            HairItemID = 0x2046; // Long Hair
            HairHue = 1157; // Ethereal blue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1150, Name = "Gown of Shattered Reflections" }); // Silvery-blue hue
            AddItem(new Cloak() { Hue = 1109, Name = "Veil of the Mirrorborn" }); // Shadowy gray hue
            AddItem(new WizardsHat() { Hue = 1154, Name = "Cowl of the Twin Moons" }); // Light bluish hue
            AddItem(new Sandals() { Hue = 1150, Name = "Steps Between Worlds" }); // Same as gown for cohesion
            AddItem(new BodySash() { Hue = 1151, Name = "Sash of Mirrorlight" }); // Slightly brighter blue
            AddItem(new MagicWand() { Hue = 1107, Name = "Shard of the Truthglass" }); // Deep azure wand
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
