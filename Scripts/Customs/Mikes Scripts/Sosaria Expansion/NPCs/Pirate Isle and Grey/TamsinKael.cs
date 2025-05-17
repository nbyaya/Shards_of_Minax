using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class HeartsOfTheUndyingQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Hearts of the Undying";

        public override object Description
        {
            get
            {
                return "Listen closely, seeker. In the lowest crypts of the Pyramid lie the Elite Mummies—guardians of the Pharaoh’s eternal court. Their hearts... still beat. " +
                       "Mummified, yes, but never stilled. I need six of them.\n\n" +
                       "With those hearts, I can complete the Rite of Veiled Blood and speak to my ancestor—to *him.* The Pharaoh. The scholars say I’m mad. " +
                       "But madness is just the name given to truths buried too deep.";
            }
        }

        public override object Refuse =>
            "A pity. The bloodline grows thinner by the hour.";

        public override object Uncomplete =>
            "The crypt is deep, and the guards restless. The Elite Mummies do not part with their hearts willingly. But I must have them. *He* is calling to me.";

        public override object Complete
        {
            get
            {
                return "Yes… I can hear it now. The drumbeat of the Veiled Blood. The hearts are still warm… still willing.\n\n" +
                       "You’ve done more than you know. Perhaps *he* will speak. Perhaps he already watches. Take this. The desert gives little—but it remembers loyalty.";
            }
        }

        public HeartsOfTheUndyingQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(MummifiedHeart), "Mummified Heart", 6, 0x024B, 2117));
            AddReward(new BaseReward(typeof(Gold), 8500, "8500 Gold"));
            AddReward(new BaseReward(typeof(RandomMagicJewelry), 1, "Pharaoh’s Band of Whispered Blood"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed the Hearts of the Undying quest!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }

    public class TamsinKael : MondainQuester
    {
        [Constructable]
        public TamsinKael() : base("Desert Historian", "Tamsin Kael")
        {
        }

        public TamsinKael(Serial serial) : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(70, 70, 100);
            Body = 401;
            Hue = 33466; // Tanned complexion
            HairItemID = 0x2047;
            HairHue = 1147;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(2117)); // Deep desert red
            AddItem(new Sandals(2401));
            AddItem(new SkullCap(2117));
            AddItem(new GoldRing { Name = "Pharaoh's Claim" });

            Backpack pack = new Backpack();
            pack.Name = "Tamsin’s Satchel";
            AddItem(pack);
        }

        public override Type[] Quests => new Type[] { typeof(HeartsOfTheUndyingQuest) };

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
