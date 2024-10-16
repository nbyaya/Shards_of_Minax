using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a dreamy ferret corpse")]
    public class DreamyFerret : BaseCreature
    {
        private DateTime m_NextDreamDust;
        private DateTime m_NextDreamWeave;
        private DateTime m_NextIllusion;
        private DateTime m_NextDreamyAura;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public DreamyFerret()
            : base(AIType.AI_Animal, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a dreamy ferret";
            Body = 0x117; // Ferret body
            Hue = 1575; // Dreamy hue (change as needed)
			BaseSoundID = 0xCF;

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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public DreamyFerret(Serial serial)
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
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextDreamDust = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextDreamWeave = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextIllusion = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextDreamyAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextDreamDust)
                {
                    DreamDust();
                }

                if (DateTime.UtcNow >= m_NextDreamWeave)
                {
                    DreamWeave();
                }

                if (DateTime.UtcNow >= m_NextIllusion)
                {
                    CreateIllusion();
                }

                if (DateTime.UtcNow >= m_NextDreamyAura)
                {
                    ApplyDreamyAura();
                }
            }
        }

        private void DreamDust()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Dreamy Ferret scatters a cloud of dreamy dust! *");
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 30, 1154);

            List<Mobile> mobilesList = new List<Mobile>();
            foreach (Mobile target in GetMobilesInRange(3))
            {
                mobilesList.Add(target);
            }
            Mobile[] mobiles = mobilesList.ToArray();

            foreach (Mobile target in mobiles)
            {
                if (target != this && target.Alive)
                {
                    target.SendMessage("You feel drowsy and confused as the dreamy dust settles over you.");
                    target.Freeze(TimeSpan.FromSeconds(5));
                    if (Utility.RandomBool())
                    {
                        target.SendMessage("You attack your allies in your confusion!");
                        target.Hits -= Utility.RandomMinMax(10, 20);
                        if (mobiles.Length > 0)
                        {
                            Mobile randomMobile = mobiles[Utility.Random(mobiles.Length)];
                            if (randomMobile != this && randomMobile != target.Combatant)
                            {
                                target.Combatant = (Mobile)randomMobile;
                            }
                        }
                    }
                }
            }

            m_NextDreamDust = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for DreamDust
        }

        private void DreamWeave()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Dreamy Ferret weaves a comforting illusion! *");
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 30, 1154);

            foreach (Mobile target in GetMobilesInRange(5))
            {
                if (target != this && target.Alive)
                {
                    target.SendMessage("You feel a soothing aura that heals your wounds and strengthens your resolve.");
                    target.Heal(15);
                    target.SendMessage("You feel stronger and more resilient!");
                    target.AddStatMod(new StatMod(StatType.Str, "Dreamy Ferret", 10, TimeSpan.FromSeconds(45)));
                    target.AddStatMod(new StatMod(StatType.Dex, "Dreamy Ferret", 10, TimeSpan.FromSeconds(45)));
                    target.AddStatMod(new StatMod(StatType.Int, "Dreamy Ferret", 10, TimeSpan.FromSeconds(45)));
                }
            }

            m_NextDreamWeave = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for DreamWeave
        }

        private void CreateIllusion()
        {
            if (Utility.RandomDouble() < 0.25)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Dreamy Ferret creates an illusionary duplicate of itself! *");
                DreamyFerret illusion = new DreamyFerret { Hue = 1154 }; // Slightly different hue for illusions
                illusion.MoveToWorld(this.Location, this.Map);
                illusion.Controlled = false;
                illusion.Blessed = true;

                Timer.DelayCall(TimeSpan.FromMinutes(1), () =>
                {
                    if (!illusion.Deleted)
                        illusion.Delete();
                });

                m_NextIllusion = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for CreateIllusion
            }
        }

        private void ApplyDreamyAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Dreamy Ferret's aura of dreams expands! *");
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x373A, 10, 30, 1154);

            foreach (Mobile target in GetMobilesInRange(5))
            {
                if (target != this && target.Alive && target.Player)
                {
                    target.SendMessage("The Dreamy Ferret's aura makes you feel disoriented and weak.");
                    target.Damage(5, this);
                    target.Freeze(TimeSpan.FromSeconds(3));
                }
            }

            m_NextDreamyAura = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for ApplyDreamyAura
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m is DreamyFerret && m.InRange(this, 3) && m.Alive)
                Talk();
        }

        private void Talk()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "The Dreamy Ferret softly whispers: *dreamy whispers*");
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
}
