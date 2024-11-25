using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the nymph queen")]
    public class NymphSingerBoss : NymphSinger
    {
        [Constructable]
        public NymphSingerBoss() : base()
        {
            Name = "Nymph Queen";
            Title = "the Songstress of Legends";

            // Update stats to match or exceed Barracoon's values
            SetStr(425); // Matching Barracoon's upper strength
            SetDex(200); // Matching Barracoon's upper dexterity
            SetInt(300); // Higher intelligence for better spellcasting

            SetHits(3500); // Significantly higher health for boss status
            SetStam(400);  // Higher stamina for better performance in combat
            SetMana(800);  // Higher mana for more spellcasting potential

            SetDamage(20, 40); // Increased damage for a boss-tier encounter

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 50.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 50.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 60; // Higher virtual armor for added defense

            // Attach a random ability for additional gameplay mechanics
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

            // Additional custom loot logic if needed
            this.Say(true, "The song... is eternal...");
        }

        public override void OnThink()
        {
            base.OnThink();
            // Further boss logic or custom actions can be added here
        }

        public NymphSingerBoss(Serial serial) : base(serial)
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
