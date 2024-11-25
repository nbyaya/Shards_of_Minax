using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the longbow overlord")]
    public class LongbowSniperBoss : LongbowSniper
    {
        [Constructable]
        public LongbowSniperBoss() : base()
        {
            Name = "Longbow Overlord";
            Title = "the Sniper King";

            // Update stats to match or exceed Barracoon's
            SetStr(425); // Matching Barracoon's upper strength
            SetDex(500); // Higher dexterity for a better archer
            SetInt(200); // Increased intelligence

            SetHits(450); // Increased health
            SetDamage(30, 45); // Increased damage

            SetResistance(ResistanceType.Physical, 45, 55); // Improved resistances
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Anatomy, 100.0, 120.0);
            SetSkill(SkillName.Archery, 120.0, 140.0); // Increased archery skill
            SetSkill(SkillName.Tactics, 120.0, 140.0); // Increased tactics
            SetSkill(SkillName.MagicResist, 70.0, 90.0); // Better resistances
            SetSkill(SkillName.Hiding, 100.0, 120.0);
            SetSkill(SkillName.Stealth, 100.0, 120.0);

            Fame = 10000; // Higher fame to reflect boss status
            Karma = -10000; // Negative karma for the boss
            VirtualArmor = 50; // Better virtual armor

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic can be added here if necessary
        }

        public LongbowSniperBoss(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
