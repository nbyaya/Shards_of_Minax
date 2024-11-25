using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the feast overlord")]
    public class FeastMasterBoss : FeastMaster
    {
        [Constructable]
        public FeastMasterBoss() : base()
        {
            Name = "Feast Overlord";
            Title = "the Supreme Feast Master";

            // Update stats to match or exceed Barracoon's
            SetStr(425); // Strength matching Barracoon's upper limit
            SetDex(300); // Dexterity at the high end
            SetInt(500); // Intelligence matching or exceeding Barracoon's upper limit

            SetHits(12000); // Set high hit points like Barracoon
            SetDamage(29, 38); // Match Barracoon's damage range

            SetResistance(ResistanceType.Physical, 70, 75); // Stronger physical resistance
            SetResistance(ResistanceType.Fire, 70, 80); // Stronger fire resistance
            SetResistance(ResistanceType.Cold, 65, 80); // Stronger cold resistance
            SetResistance(ResistanceType.Poison, 70, 75); // Stronger poison resistance
            SetResistance(ResistanceType.Energy, 70, 80); // Stronger energy resistance

            SetSkill(SkillName.EvalInt, 100.0, 120.0); // Enhance the magic skill
            SetSkill(SkillName.Magery, 100.0, 120.0); // Enhance the magery skill
            SetSkill(SkillName.Meditation, 120.0, 140.0); // Strong meditation skill
            SetSkill(SkillName.MagicResist, 95.0, 105.0); // Strong magic resist
            SetSkill(SkillName.Tactics, 80.0, 100.0); // Enhance tactics
            SetSkill(SkillName.Wrestling, 70.0, 90.0); // Moderate wrestling skill

            Fame = 22500; // Increased fame to match a boss-tier creature
            Karma = -22500; // Negative karma (like Barracoon)
            VirtualArmor = 70; // Strong virtual armor to improve survivability

            // Attach random ability to the boss using XML-based system
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

            // Boss sayings after death
            int phrase = Utility.Random(2);
            switch (phrase)
            {
                case 0: this.Say(true, "My feast... ruined..."); break;
                case 1: this.Say(true, "I... will return..."); break;
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss-specific logic can be added here, if necessary
        }

        public FeastMasterBoss(Serial serial) : base(serial)
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
