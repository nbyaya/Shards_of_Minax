using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a shadowy sludge corpse")]
    public class ShadowSludge : BaseCreature
    {
        private DateTime m_NextShadowMeld;
        private DateTime m_NextDrainLife;
        private DateTime m_NextDarknessAura;
        private DateTime m_NextShadowSwarm;
        private bool m_DarknessAuraActive;

        [Constructable]
        public ShadowSludge()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Shadow Sludge";
            Body = 51; // Slime body
            Hue = 2380; // Dark hue
			BaseSoundID = 456;

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

            m_DarknessAuraActive = false;
        }

        public ShadowSludge(Serial serial)
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
				if (DateTime.UtcNow >= m_NextShadowMeld)
				{
					ShadowMeld();
				}

				if (DateTime.UtcNow >= m_NextDrainLife)
				{
					DrainLife();
				}

				if (DateTime.UtcNow >= m_NextDarknessAura)
				{
					DarknessAura();
				}

				if (DateTime.UtcNow >= m_NextShadowSwarm)
				{
					ShadowSwarm();
				}

				// Ensure Combatant is not null before checking the range
				if (m_DarknessAuraActive && Combatant != null && !InRange(Combatant, 5))
				{
					DeactivateDarknessAura();
				}
			}
		}


        private void ShadowMeld()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Shadow Sludge vanishes into the shadows! *");
            PlaySound(0x20D); // Stealth sound

            this.Hidden = true;

            // Teleport to a nearby location
            Point3D newLocation = new Point3D(Location.X + Utility.RandomMinMax(-5, 5), Location.Y + Utility.RandomMinMax(-5, 5), Location.Z);
            if (Map.CanFit(newLocation, 16, false, false))
            {
                this.Location = newLocation;
            }

            Timer.DelayCall(TimeSpan.FromSeconds(2), () =>
            {
                if (Combatant != null)
                {
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Shadow Sludge reappears with a powerful strike! *");
                    PlaySound(0x20D); // Stealth sound

                    this.Hidden = false;
                    Attack(Combatant);

                    m_NextShadowMeld = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for ShadowMeld
                }
            });
        }

        private void DrainLife()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Shadow Sludge drains the life from its victim! *");
                PlaySound(0x231); // Life drain sound

                int damage = Utility.RandomMinMax(10, 20);
                AOS.Damage(Combatant, this, damage, 0, 0, 100, 0, 0);

                Hits += damage / 2; // Heal for half the damage done

				// No damage over time effect here, just a message
				Mobile mobile = Combatant as Mobile;
				if (mobile != null)
				{
					mobile.SendMessage("You feel your life force being drained!");
					mobile.SendMessage("You feel weaker and less capable of fighting!");
					mobile.SendMessage("You are suffering from a lingering pain.");
				}

                m_NextDrainLife = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for DrainLife
            }
        }

        private void DarknessAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Shadow Sludge releases an aura of darkness! *");
            PlaySound(0x20D); // Dark aura sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m != Combatant)
                {
                    m.SendMessage("The darkness surrounding Shadow Sludge clouds your vision!");
                    m.SendMessage("Your accuracy is diminished!");

                    // Reduce accuracy (this is a placeholder for actual debuff logic)
                    Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
                    {
                        // Restore accuracy (this is a placeholder for actual debuff restoration logic)
                    });
                }
            }

            m_DarknessAuraActive = true;
            m_NextDarknessAura = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for DarknessAura
        }

        private void DeactivateDarknessAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The darkness surrounding Shadow Sludge fades away. *");
            m_DarknessAuraActive = false;
        }

        private void ShadowSwarm()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Shadow Sludge summons a swarm of shadowy minions! *");
            PlaySound(0x20D); // Summoning sound

            for (int i = 0; i < 3; i++)
            {
                ShadowSlimeMinion minion = new ShadowSlimeMinion();
                Point3D minionLocation = new Point3D(Location.X + Utility.RandomMinMax(-2, 2), Location.Y + Utility.RandomMinMax(-2, 2), Location.Z);
                minion.MoveToWorld(minionLocation, Map);
                minion.Combatant = Combatant;
            }

            m_NextShadowSwarm = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for ShadowSwarm
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

    public class ShadowSlimeMinion : BaseCreature
    {
        [Constructable]
        public ShadowSlimeMinion()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "shadow minion";
            Body = 51; // Slime body
            Hue = 1152; // Dark hue

            SetStr(30, 50);
            SetDex(20, 30);
            SetInt(10, 20);

            SetHits(30, 50);
            SetMana(0);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 10, 20);
            SetResistance(ResistanceType.Poison, 20, 30);

            SetSkill(SkillName.MagicResist, 30.0, 50.0);
            SetSkill(SkillName.Tactics, 40.0, 60.0);
            SetSkill(SkillName.Wrestling, 40.0, 60.0);

            Fame = 1000;
            Karma = -1000;

            VirtualArmor = 10;
        }

        public ShadowSlimeMinion(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !InRange(Combatant, 1))
                return;

            // You can add specific logic here if needed
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
