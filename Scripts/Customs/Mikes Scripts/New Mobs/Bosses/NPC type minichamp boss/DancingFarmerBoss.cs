using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the dancing overlord")]
    public class DancingFarmerBoss : DancingFarmerC
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(5.0); // Shorter speech delay for the boss

        [Constructable]
        public DancingFarmerBoss() : base()
        {
            Name = "Dancing Overlord";
            Title = "the Supreme Dancer";

            // Update stats to match or exceed Barracoon's
            SetStr(425); // Matching Barracoon's strength
            SetDex(150); // Matching Barracoon's dexterity
            SetInt(750); // Matching Barracoon's intelligence

            SetHits(12000); // Matching Barracoon's health
            SetStam(300); // Matching Barracoon's stamina
            SetMana(750); // Matching Barracoon's mana

            SetDamage(29, 38); // Matching Barracoon's damage range

            SetResistance(ResistanceType.Physical, 70, 75);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 65, 80);
            SetResistance(ResistanceType.Poison, 70, 75);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            m_SpeechDelay = TimeSpan.FromSeconds(5.0); // Speed up speech for boss interaction

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

            // Summon cows on death, just like the original DancingFarmerC
            for (int i = 0; i < Utility.RandomMinMax(2, 5); ++i)
            {
                BaseCreature cow = new Cow();
                cow.MoveToWorld(this.Location, this.Map);
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Optionally add more dynamic actions here
        }

        public override int Damage(int amount, Mobile from)
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
            {
                // Shortened speech delay for boss
                if (Utility.RandomBool())
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "Watch the dance of destruction!"); break;
                        case 1: this.Say(true, "The rhythm of battle cannot be stopped!"); break;
                        case 2: this.Say(true, "My dance is my strength!"); break;
                        case 3: this.Say(true, "The fields echo with my power!"); break;
                    }

                }
            }

            return base.Damage(amount, from);
        }

        public DancingFarmerBoss(Serial serial) : base(serial)
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
