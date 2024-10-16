using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a Dia de los Muertos llama corpse")]
    public class DiaDeLosMuertosLlama : BaseCreature
    {
        private DateTime m_NextSpiritsBlessing;
        private DateTime m_NextSugarSkullToss;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public DiaDeLosMuertosLlama()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Dia de los Muertos llama";
            Body = 0xDC; // Llama body
            Hue = 1154; // Unique festive hue with skull patterns
			this.BaseSoundID = 0x3F3;

            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
			
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;

            m_AbilitiesInitialized = false;
        }

        public DiaDeLosMuertosLlama(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextSpiritsBlessing = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextSugarSkullToss = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextSpiritsBlessing)
                {
                    SpiritsBlessing();
                }

                if (DateTime.UtcNow >= m_NextSugarSkullToss)
                {
                    SugarSkullToss();
                }
            }
        }

        private void SpiritsBlessing()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Dia de los Muertos Llama invokes ancestral spirits for a blessing! *");
            PlaySound(0x1B9); // Magical sound

            // Heal and provide a defensive boost
            this.Heal(30);
            this.VirtualArmor += 15;

            // Summon spirit minions
            for (int i = 0; i < 2; i++)
            {
                BaseCreature spirit = new SpiritOfTheDead();
                spirit.Team = this.Team;
                spirit.MoveToWorld(Location, Map);
            }

            m_NextSpiritsBlessing = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown
        }

        private void SugarSkullToss()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Dia de los Muertos Llama throws a sugar skull! *");
            PlaySound(0x1E3); // Throw sound

            Point3D loc = Location;
            SugarSkullItem skull = new SugarSkullItem();
            skull.MoveToWorld(loc, Map);

            Timer.DelayCall(TimeSpan.FromSeconds(2), () => ExplodeSugarSkull(skull));

            m_NextSugarSkullToss = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown
        }

        private void ExplodeSugarSkull(SugarSkullItem skull)
        {
            if (skull.Deleted)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The sugar skull explodes in a burst of icy cold! *");
            PlaySound(0x307); // Explosion sound

            Effects.SendLocationEffect(skull.Location, Map, 0x36BD, 20, 10); // Explosion effect

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, damage, 0, 0, 100, 0, 0); // Cold damage

                    if (Utility.RandomDouble() < 0.5) // 50% chance to freeze
                    {
                        m.Freeze(TimeSpan.FromSeconds(2));
                        m.SendMessage("You are frozen by the exploding sugar skull!");
                    }

                    if (m is Mobile mobile)
                    {
                        mobile.SendMessage("You are struck by the explosive sugar skull!");
                    }
                    m.PlaySound(0x1DD); // Explosion sound
                }
            }

            skull.Delete();
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Deleted || !Alive)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Dia de los Muertos Llama is revived by the spirits! *");
            PlaySound(0x1E1); // Revival sound

            // Revival visual effect
            FixedParticles(0x373A, 9, 32, 5030, EffectLayer.Waist);

            // Revive with half health and apply a buff
            Hits = HitsMax / 2;
            VirtualArmor = 30; // Reset virtual armor

            Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
            {
                if (Deleted || !Alive)
                    return;

                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Dia de los Muertos Llama rises from the dead with renewed strength! *");
                Hits = HitsMax / 2;

                // Apply a temporary buff to increase damage or defense
                this.SetDamage(12, 18);
                this.VirtualArmor = 40;
            });
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }

    public class SugarSkullItem : Item
    {
        public SugarSkullItem() : base(0x171D) // Use a suitable item ID for the skull
        {
            Movable = false;
        }

        public SugarSkullItem(Serial serial) : base(serial)
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

    public class SpiritOfTheDead : BaseCreature
    {
        [Constructable]
        public SpiritOfTheDead()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a spirit of the dead";
            Body = Utility.RandomList(50, 56);
            Hue = 1152; // Ghostly blue hue

            SetStr(50, 75);
            SetDex(30, 50);
            SetInt(50, 75);

            SetHits(30, 50);
            SetMana(100);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 10, 20);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.MagicResist, 50.0, 70.0);
            SetSkill(SkillName.Tactics, 50.0, 70.0);
            SetSkill(SkillName.Wrestling, 50.0, 70.0);

            Fame = 1000;
            Karma = 1000;

            VirtualArmor = 20;

            Tamable = false;
        }

        public SpiritOfTheDead(Serial serial)
            : base(serial)
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
