using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.Quests
{
    public class OrrynCindershade : MondainQuester
    {
        [Constructable]
        public OrrynCindershade() : base("Firebrand Preacher", "Orryn Cindershade")
        {
        }

        public override void InitBody()
        {
            InitStats(75, 65, 85);
            Body = 0x190;
            Hue = 2211; // slightly scorched
            HairItemID = 0x2048;
            HairHue = 1175;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe(1175)); // fire hue
            AddItem(new Sandals(33));
            AddItem(new Spellbook() { Name = "Scorched Sermons", Hue = 1175 });
        }

        public override Type[] Quests => new Type[] { typeof(AshesOfTheDamnedQuest) };

        public OrrynCindershade(Serial serial) : base(serial) { }

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
