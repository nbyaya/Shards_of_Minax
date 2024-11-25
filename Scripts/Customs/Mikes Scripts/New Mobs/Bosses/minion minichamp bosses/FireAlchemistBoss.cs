using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the fire alchemist lord")]
    public class FireAlchemistBoss : FireAlchemist
    {
        [Constructable]
        public FireAlchemistBoss() : base()
        {
            Name = "Fire Alchemist Lord";
            Title = "the Supreme Alchemist";

            // Update stats to match or exceed Barracoon (or better)
            SetStr(800); // Higher strength for a boss-tier challenge
            SetDex(250); // Higher dexterity for quicker reactions
            SetInt(400); // High intelligence for magical prowess

            SetHits(12000); // Much higher health for the boss-tier version
            SetDamage(29, 38); // Increase damage to make it a tougher opponent

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Fire, 75);

            SetResistance(ResistanceType.Physical, 60, 80); // Boss-tier resistances
            SetResistance(ResistanceType.Fire, 90, 100); // Stronger fire resistance
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 55, 75);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.EvalInt, 120.0); // Higher skill values for the boss
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 110.0);
            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 80.0);
            SetSkill(SkillName.Wrestling, 70.0);

            Fame = 25000; // Increased fame to indicate higher rank
            Karma = -25000; // Increased negative karma for the boss
            VirtualArmor = 60; // Increased virtual armor for more protection

            // Attach the random ability (XmlRandomAbility) for added complexity
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
            // You can add additional behaviors here for special boss mechanics
        }

        public FireAlchemistBoss(Serial serial) : base(serial)
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
