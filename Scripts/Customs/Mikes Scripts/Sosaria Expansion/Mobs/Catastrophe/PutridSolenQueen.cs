using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a putrid solen queen corpse")]
    public class PutridSolenQueen : BaseCreature, IBlackSolen
    {
        private bool m_BurstSac; // whether her burst sac has already been triggered
        private bool m_VenomFrenzyActive; // flag for frenzy state
        private DateTime m_NextAcidAbility; // cooldown for her directional acid attack
        private DateTime m_NextCloudAbility; // cooldown for her corrosive cloud ability

        // Constructor
        [Constructable]
        public PutridSolenQueen()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "the putrid solen queen";
            this.Body = 807;
            this.BaseSoundID = 959;
            this.Hue = 0x4A0; // Unique putrid hue (adjust as desired)


            // Advanced stats
            this.SetStr(400, 450);
            this.SetDex(200, 250);
            this.SetInt(150, 200);

            this.SetHits(600, 700);

            this.SetDamage(15, 25);

            // Mixed damage types
            this.SetDamageType(ResistanceType.Physical, 50);
            this.SetDamageType(ResistanceType.Poison, 50);

            // Resistances
            this.SetResistance(ResistanceType.Physical, 40, 50);
            this.SetResistance(ResistanceType.Fire, 40, 50);
            this.SetResistance(ResistanceType.Cold, 35, 45);
            this.SetResistance(ResistanceType.Poison, 50, 60);
            this.SetResistance(ResistanceType.Energy, 35, 45);

            // Skills (increased for an advanced monster)
            this.SetSkill(SkillName.MagicResist, 80.0, 90.0);
            this.SetSkill(SkillName.Tactics, 100.0, 110.0);
            this.SetSkill(SkillName.Wrestling, 100.0, 110.0);

            this.Fame = 6000;
            this.Karma = -6000;

            this.VirtualArmor = 60;

            // Pack some thematic loot
            this.PackItem(new ZoogiFungus((Utility.RandomDouble() > 0.05) ? 5 : 25));
            if (Utility.RandomDouble() < 0.02)
                this.PackItem(new BallOfSummoning());

            // Initialize ability cooldowns
            m_NextAcidAbility = DateTime.UtcNow;
            m_NextCloudAbility = DateTime.UtcNow;
        }

        public PutridSolenQueen(Serial serial)
            : base(serial)
        {
        }

        // Sound overrides (using same sounds as the original for consistency)
        public override int GetAngerSound() { return 0x259; }
        public override int GetIdleSound() { return 0x259; }
        public override int GetAttackSound() { return 0x195; }
        public override int GetHurtSound() { return 0x250; }
        public override int GetDeathSound() { return 0x25B; }

        // Unique Ability: Corrosive Acid Attack (similar to acid breath but with extra AoE effects)
        public void BeginCorrosiveAcid()
        {
            if (!(Combatant is Mobile target) || target.Deleted || !target.Alive || !Alive || DateTime.UtcNow < m_NextAcidAbility || !CanBeHarmful(target))
                return;

            // Play distinctive acid sound
            PlaySound(0x118);
            // Launch a moving acid effect toward the target
            MovingEffect(target, 0x36D4, 1, 0, false, false, 0x3F, 0);

            // Calculate delay based on distance
            TimeSpan delay = TimeSpan.FromSeconds(GetDistanceToSqrt(target) / 5.0);
            Timer.DelayCall<Mobile>(delay, EndCorrosiveAcid, target);

            // Set next acid ability cooldown
            m_NextAcidAbility = DateTime.UtcNow + TimeSpan.FromSeconds(5);
        }

        public void EndCorrosiveAcid(Mobile target)
        {
            if (target == null || target.Deleted || !target.Alive || !Alive)
                return;

            // 20% chance to apply an extra potent poison effect
            if (Utility.RandomDouble() <= 0.20)
                target.ApplyPoison(this, Poison.Greater);

            // Damage: part acid burst and part poison damage
            AOS.Damage(target, Utility.RandomMinMax(100, 120), 0, 0, 0, 80, 20);
        }

        // Unique Ability: Corrosive Cloud (AoE effect affecting all nearby targets)
        public void BeginCorrosiveCloud()
        {
            if (!(Combatant is Mobile)) return;
            if (DateTime.UtcNow < m_NextCloudAbility)
                return;

            // Broadcast the cloud creation
            this.Say("* The putrid solen queen releases a noxious cloud! *");
            PlaySound(0x11C); // Use a different sound for the cloud

            // Find all mobiles within a 3-tile radius
            ArrayList list = new ArrayList();
            foreach (Mobile m in this.GetMobilesInRange(3))
            {
                if (m != this && CanBeHarmful(m))
                    list.Add(m);
            }

            // For each mobile, delay a short time to simulate the cloud settling
            foreach (Mobile m in list)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                {
                    if (m == null || m.Deleted || !m.Alive || !Alive)
                        return;
                    // The cloud deals damage and applies a poison effect
                    AOS.Damage(m, Utility.RandomMinMax(50, 70), 0, 0, 0, 80, 20);
                    m.ApplyPoison(this, Poison.Deadly);
                });
            }

            // Set cooldown for the cloud ability (8-12 seconds)
            m_NextCloudAbility = DateTime.UtcNow + TimeSpan.FromSeconds(8 + (4 * Utility.RandomDouble()));
        }

        // Unique Ability: Venomous Frenzy – when hit below half health, she enters a frenzy boosting damage and attack speed temporarily.
        public void CheckForVenomousFrenzy()
        {
            if (!m_VenomFrenzyActive && Hits < (HitsMax / 2))
            {
                m_VenomFrenzyActive = true;
                this.Say("* The putrid solen queen becomes enraged, her venom intensifies! *");

                // Temporarily boost her damage and speed for 10 seconds
                double oldMin = this.DamageMin, oldMax = this.DamageMax;
                this.DamageMin = (int)(oldMin * 1.5);
                this.DamageMax = (int)(oldMax * 1.5);
                this.Combatant = Combatant; // refresh combat status, if needed

                // Use a timer to end the frenzy after 10 seconds
                Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
                {
                    // Revert back to the original damage values
                    this.DamageMin = (int)(oldMin);
                    this.DamageMax = (int)(oldMax);
                    m_VenomFrenzyActive = false;
                    this.Say("* The queen's fury subsides. *");
                });
            }
        }

        // Unique passive: Putrid Aura – any enemy in melee range gets a tick of acid damage over time.
        public void ApplyPutridAura()
        {
            ArrayList list = new ArrayList();
            foreach (Mobile m in this.GetMobilesInRange(1))
            {
                if (m != this && CanBeHarmful(m))
                    list.Add(m);
            }

            foreach (Mobile m in list)
            {
                // Apply a small amount of acid damage and a slight poison effect
                AOS.Damage(m, Utility.RandomMinMax(10, 15), 0, 0, 0, 90, 10);
                m.ApplyPoison(this, Poison.Regular);
            }
        }

        // Override melee attack responses to trigger abilities
        public override void OnGotMeleeAttack(Mobile attacker)
        {
            // If the attacker is using a ranged weapon, trigger the directional corrosive acid attack.
            if (attacker.Weapon is BaseRanged)
                BeginCorrosiveAcid();
            else if (this.Map != null && attacker != this && !m_BurstSac && Utility.RandomDouble() < 0.20)
            {
                // Summon a burst sac for additional spawn support
                PutridEggSac sac = new PutridEggSac();
                sac.MoveToWorld(this.Location, this.Map);
                PlaySound(0x582);
                Say("* The queen summons her hideous spawn! *");
                m_BurstSac = true;
                new EggSacResetTimer(this).Start();
            }

            base.OnGotMeleeAttack(attacker);
        }

        // Override spell damage handling to sometimes trigger corrosive acid
        public override void OnDamagedBySpell(Mobile attacker)
        {
            base.OnDamagedBySpell(attacker);

            if (Utility.RandomDouble() <= 0.80)
                BeginCorrosiveAcid();
        }

        // Override OnDamage so that if she gets attacked by melee, she can apply her putrid aura and check for frenzy
        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            // If not about to die, apply a spill effect on adjacent foes
            if (!willKill)
            {
                if (!m_BurstSac && this.Hits < 100)
                {
                    this.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The queen's toxic sac ruptures! *");
                    m_BurstSac = true;
                }
                else if (from != null && from != this && InRange(from, 1))
                {
                    // Spill a small spray of venom on the attacker
                    AOS.Damage(from, Utility.RandomMinMax(20, 30), 0, 0, 0, 100, 0);
                }
            }

            // Each time she takes damage, check if she should enter Venomous Frenzy
            CheckForVenomousFrenzy();

            // Also, apply her passive putrid aura to punishing melee attackers
            if (from != null && from != this && InRange(from, 1))
                ApplyPutridAura();

            base.OnDamage(amount, from, willKill);
        }

        // Override OnActionCombat to occasionally trigger her corrosive cloud attack
        public override void OnActionCombat()
        {
            if (!(Combatant is Mobile)) 
                return;

            Mobile combatant = Combatant as Mobile;
            if (combatant == null || combatant.Deleted || combatant.Map != Map || !InRange(combatant, 12) || !CanBeHarmful(combatant) || !InLOS(combatant))
                return;

            if (Utility.RandomDouble() < 0.30)
                BeginCorrosiveCloud();

            base.OnActionCombat();
        }

        // Override OnBeforeDeath to trigger a deadly necrotic burst on death
        public override bool OnBeforeDeath()
        {
            // Before dying, spill a massive acid explosion to damage all nearby foes
            ArrayList list = new ArrayList();
            foreach (Mobile m in this.GetMobilesInRange(2))
            {
                if (m != this && CanBeHarmful(m))
                    list.Add(m);
            }
            foreach (Mobile m in list)
            {
                AOS.Damage(m, Utility.RandomMinMax(80, 100), 0, 0, 0, 100, 0);
                m.ApplyPoison(this, Poison.Deadly);
            }

            return base.OnBeforeDeath();
        }

        public override void GenerateLoot()
        {
            // Advanced loot: use an UltraRich loot pack plus some potions.
            this.AddLoot(LootPack.UltraRich);
            this.AddLoot(LootPack.Potions);
            // Very rare unique drop chance
            if (Utility.RandomDouble() < 0.001)
            {
                this.PackItem(new PutridSolenCrown());
            }
        }

        // Serialization
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
            writer.Write(m_BurstSac);
            writer.Write(m_VenomFrenzyActive);
            // No need to save cooldown timestamps; they can be reset on deserialization
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        m_BurstSac = reader.ReadBool();
                        m_VenomFrenzyActive = reader.ReadBool();
                        break;
                    }
            }
        }

        // Reset timer for burst sac flag so that she can summon spawn again after a delay
		private class EggSacResetTimer : Timer
		{
			private PutridSolenQueen m_Queen;

			public EggSacResetTimer(PutridSolenQueen queen) : base(TimeSpan.FromSeconds(10))
			{
				m_Queen = queen;
				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
				if (m_Queen != null && !m_Queen.Deleted)
				{
					m_Queen.m_BurstSac = false;
				}
			}
		}

    }

    // A unique egg sac that spawns advanced worker monsters (Putrid Solen Spawn) for the queen.
    public class PutridEggSac : Item, ICarvable
    {
        private SpawnTimer m_Timer;

        public override string DefaultName { get { return "putrid egg sac"; } }

        [Constructable]
        public PutridEggSac()
            : base(4316)
        {
            Movable = false;
            Hue = 0x500; // A unique hue for the putrid sac
            m_Timer = new SpawnTimer(this);
            m_Timer.Start();
        }

        public bool Carve(Mobile from, Item item)
        {
            Effects.PlaySound(GetWorldLocation(), Map, 0x027);
            Effects.SendLocationEffect(GetWorldLocation(), Map, 0x3728, 10, 10, 0, 0);
            from.SendMessage("You destroy the egg sac.");
            Delete();
            m_Timer.Stop();
            return true;
        }

        public PutridEggSac(Serial serial)
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
            m_Timer = new SpawnTimer(this);
            m_Timer.Start();
        }

        private class SpawnTimer : Timer
        {
            private Item m_Item;
            public SpawnTimer(Item item)
                : base(TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10)))
            {
                Priority = TimerPriority.FiftyMS;
                m_Item = item;
            }
            protected override void OnTick()
            {
                if (m_Item.Deleted)
                    return;

                Mobile spawn;
                // Randomly choose one of two advanced worker types
                switch (Utility.Random(2))
                {
                    case 0:
                        spawn = new PutridSolenWarrior();
                        break;
                    default:
                        spawn = new PutridSolenWorker();
                        break;
                }
                spawn.MoveToWorld(m_Item.Location, m_Item.Map);
                m_Item.Delete();
            }
        }
    }

    // Placeholder classes for the advanced spawn types and a unique drop.
    public class PutridSolenWarrior : BaseCreature
    {
        [Constructable]
        public PutridSolenWarrior()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a putrid solen warrior";
            this.Body = 807;
            this.Hue = 0x4A0;
            this.BaseSoundID = 959;

            this.SetStr(200, 250);
            this.SetDex(150, 175);
            this.SetInt(50, 75);
            this.SetHits(300, 350);
            this.SetDamage(8, 12);
            this.SetDamageType(ResistanceType.Physical, 60);
            this.SetDamageType(ResistanceType.Poison, 40);

            this.SetResistance(ResistanceType.Physical, 30, 40);
            this.SetResistance(ResistanceType.Poison, 40, 50);

            this.SetSkill(SkillName.MagicResist, 60, 70);
            this.SetSkill(SkillName.Tactics, 80, 90);
            this.SetSkill(SkillName.Wrestling, 80, 90);

            this.Fame = 3000;
            this.Karma = -3000;
            this.VirtualArmor = 30;
        }

        public PutridSolenWarrior(Serial serial)
            : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class PutridSolenWorker : BaseCreature
    {
        [Constructable]
        public PutridSolenWorker()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a putrid solen worker";
            this.Body = 807;
            this.Hue = 0x4A0;
            this.BaseSoundID = 959;

            this.SetStr(150, 175);
            this.SetDex(130, 150);
            this.SetInt(40, 60);
            this.SetHits(250, 300);
            this.SetDamage(6, 10);
            this.SetDamageType(ResistanceType.Physical, 50);
            this.SetDamageType(ResistanceType.Poison, 50);

            this.SetResistance(ResistanceType.Physical, 25, 35);
            this.SetResistance(ResistanceType.Poison, 35, 45);

            this.SetSkill(SkillName.MagicResist, 50, 60);
            this.SetSkill(SkillName.Tactics, 70, 80);
            this.SetSkill(SkillName.Wrestling, 70, 80);

            this.Fame = 2500;
            this.Karma = -2500;
            this.VirtualArmor = 25;
        }

        public PutridSolenWorker(Serial serial)
            : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    // A very rare unique drop from the Putrid Solen Queen.
    public class PutridSolenCrown : Item
    {
        [Constructable]
        public PutridSolenCrown() : base(0x1F14)
        {
            this.Weight = 1.0;
            this.Hue = 0x4A0;
            this.Name = "The Crown of Putrid Sovereignty";
        }

        public PutridSolenCrown(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
