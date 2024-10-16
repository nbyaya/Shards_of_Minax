using System;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a cursed white-tail corpse")]
    public class CursedWhiteTail : BaseCreature
    {
        private DateTime m_NextCurseOfTheWhiteTail;
        private DateTime m_NextSpectralCharge;
        private DateTime m_NextSummonSpirits;

        [Constructable]
        public CursedWhiteTail()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a cursed white-tail";
            Body = 0xEA; // GreatHart body
            Hue = 1997; // Ghostly hue

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

            m_NextCurseOfTheWhiteTail = DateTime.UtcNow;
            m_NextSpectralCharge = DateTime.UtcNow;
            m_NextSummonSpirits = DateTime.UtcNow;
        }

        public CursedWhiteTail(Serial serial)
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
        public override int GetAttackSound() 
        { 
            return 0x82; 
        }

        public override int GetHurtSound() 
        { 
            return 0x83; 
        }

        public override int GetDeathSound() 
        { 
            return 0x84; 
        }
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextCurseOfTheWhiteTail)
                {
                    CurseOfTheWhiteTail();
                }

                if (DateTime.UtcNow >= m_NextSpectralCharge)
                {
                    SpectralCharge();
                }

                if (DateTime.UtcNow >= m_NextSummonSpirits)
                {
                    SummonCursedSpirits();
                }
            }
        }

        private void CurseOfTheWhiteTail()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Cursed White-tail casts a dark curse upon you!*");

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m != Combatant)
                {
                    m.SendMessage("You are enveloped by a dark curse, sapping your strength!");
                    // Implement logic to reduce combat abilities and apply periodic damage
                    Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
                    {
                        if (m != null && m.Alive)
                        {
                            AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                            m.SendMessage("The curse continues to ravage your body!");
                        }
                    });

                    m_NextCurseOfTheWhiteTail = DateTime.UtcNow + TimeSpan.FromSeconds(30);
                }
            }
        }

		private void SpectralCharge()
		{
			PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Cursed White-tail charges through you with ghostly speed!*");

			foreach (Mobile m in GetMobilesInRange(5))
			{
				if (m != this && m != Combatant)
				{
					m.SendMessage("You are struck by a ghostly force, leaving you in a state of fear!");
					// Cast 'm' to 'Mobile' to access 'Freeze'
					Mobile mobile = m as Mobile;
					if (mobile != null)
					{
						mobile.Freeze(TimeSpan.FromSeconds(2)); // Temporarily freezes the target
					}

					m_NextSpectralCharge = DateTime.UtcNow + TimeSpan.FromMinutes(1);
				}
			}
		}


        private void SummonCursedSpirits()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Cursed White-tail summons cursed spirits to aid it in battle!*");

            for (int i = 0; i < 2; i++)
            {
                CursedSpirit spirit = new CursedSpirit();
                spirit.MoveToWorld(Location, Map);
                spirit.Combatant = Combatant;
            }

            m_NextSummonSpirits = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        public override void OnDamagedBySpell(Mobile caster)
        {
            base.OnDamagedBySpell(caster);

            // Add reaction to being damaged by spells
            if (Utility.RandomDouble() < 0.1) // 10% chance
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Cursed White-tail retaliates with a burst of dark energy!*");
                caster.SendMessage("You are struck by a burst of dark energy from the Cursed White-tail!");
                caster.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Head);
                AOS.Damage(caster, this, Utility.RandomMinMax(5, 15), 0, 100, 0, 0, 0);
                caster.PlaySound(0x1F5);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write(m_NextCurseOfTheWhiteTail);
            writer.Write(m_NextSpectralCharge);
            writer.Write(m_NextSummonSpirits);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextCurseOfTheWhiteTail = reader.ReadDateTime();
            m_NextSpectralCharge = reader.ReadDateTime();
            m_NextSummonSpirits = reader.ReadDateTime();
        }
    }

    public class CursedSpirit : BaseCreature
    {
        [Constructable]
        public CursedSpirit()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a cursed spirit";
            Body = 0x9A; // Ghost body
            Hue = 1153; // Ghostly hue

            SetStr(50, 70);
            SetDex(40, 60);
            SetInt(30, 50);

            SetHits(40, 60);
            SetMana(0);

            SetDamage(8, 12);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 25);
            SetResistance(ResistanceType.Cold, 10, 20);

            SetSkill(SkillName.MagicResist, 30.0, 50.0);
            SetSkill(SkillName.Tactics, 30.0, 50.0);
            SetSkill(SkillName.Wrestling, 30.0, 50.0);

            Fame = 500;
            Karma = -500;

            VirtualArmor = 20;

            Tamable = false;
        }

        public CursedSpirit(Serial serial)
            : base(serial)
        {
        }

		public override void OnThink()
		{
			base.OnThink();

			if (Combatant != null)
			{
				if (Utility.RandomDouble() < 0.05) // 5% chance to inflict fear
				{
					
					Mobile mobile = Combatant as Mobile;
					
					if (mobile != null)
					{
						mobile.Freeze(TimeSpan.FromSeconds(2)); // Temporarily freezes the target
						mobile.SendMessage("You are overwhelmed by a spectral force, leaving you frightened!");
					// Cast 'Combatant' to 'Mobile' to access 'Freeze'
					}
				}
			}
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
