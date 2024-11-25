using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the explosive overlord")]
    public class ExplosiveDemolitionistBoss : ExplosiveDemolitionist
    {
        [Constructable]
        public ExplosiveDemolitionistBoss() : base()
        {
            Name = "Explosive Overlord";
            Title = "the Supreme Demolitionist";

            // Update stats to match or exceed Barracoon
            SetStr(1000); // Upper bound for strength
            SetDex(200); // Upper bound for dexterity
            SetInt(150); // Keeping intelligence in a high range

            SetHits(12000); // Boss health to match Barracoon
            SetDamage(29, 38); // Matching or exceeding Barracoon's damage range

            SetResistance(ResistanceType.Physical, 70, 75);
            SetResistance(ResistanceType.Fire, 80, 90); // Increased Fire resistance
            SetResistance(ResistanceType.Cold, 50, 60); // Increased Cold resistance
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.MagicResist, 100.0); // Higher Magic Resist
            SetSkill(SkillName.Tactics, 120.0); // Enhanced Tactics
            SetSkill(SkillName.Macing, 120.0); // Enhanced Macing

            Fame = 22500; // Higher fame
            Karma = -22500; // Higher karma (negative for a boss)

            VirtualArmor = 70; // Enhanced armor

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

			PackItem(new BossTreasureBox());
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new TalismanSlotChangeDeed());
			PackItem(new LavaLizardSummoningMateria());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here, if needed
        }

        public ExplosiveDemolitionistBoss(Serial serial) : base(serial)
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
