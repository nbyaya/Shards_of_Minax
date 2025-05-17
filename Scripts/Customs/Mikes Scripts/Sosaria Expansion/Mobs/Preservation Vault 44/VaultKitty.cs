using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a vault kitty corpse")]
    public class VaultKitty : BaseCreature
    {
        private DateTime m_NextPounceTime;
        private DateTime m_NextSurgeTime;
        private DateTime m_NextDecoyTime;
        private int m_LivesRemaining = 3;

        private const int UniqueHue = 1157; // A strange neon-green glow

        [Constructable]
        public VaultKitty()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Vault Kitty";
            Body = 243;                 // Reuse the LesserHiryu body
            BaseSoundID = 0x4FD;        // Idle sound base
            Hue = UniqueHue;

            // Stats
            SetStr(200, 250);
            SetDex(300, 350);
            SetInt(200, 250);

            SetHits(800, 1000);
            SetStam(300, 350);
            SetMana(200, 300);

            SetDamage(20, 30);

            // Damage types
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Energy, 60);

            // Resistances
            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire,     40, 50);
            SetResistance(ResistanceType.Cold,     50, 60);
            SetResistance(ResistanceType.Poison,   60, 80);
            SetResistance(ResistanceType.Energy,   40, 50);

            // Skills
            SetSkill(SkillName.EvalInt,      90.1, 100.0);
            SetSkill(SkillName.Magery,       90.1, 100.0);
            SetSkill(SkillName.MagicResist, 100.1, 110.0);
            SetSkill(SkillName.Meditation,   80.0,  90.0);
            SetSkill(SkillName.Tactics,      95.1, 105.0);
            SetSkill(SkillName.Wrestling,    95.1, 105.0);

            Fame = 15000;
            Karma = -15000;
            VirtualArmor = 60;
            ControlSlots = 6;

            // Initialize cooldowns
            m_NextPounceTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextSurgeTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextDecoyTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));

            // Base loot
            PackItem(new PowerCrystal(Utility.RandomMinMax(3, 6)));
            PackItem(new VoidCore(Utility.RandomMinMax(1, 2)));
        }

        // --- Nine Lives Mechanic ---
        public override bool OnBeforeDeath()
        {
            if (m_LivesRemaining > 0)
            {
                m_LivesRemaining--;
                // Momentary revival
                PlaySound(0x1FE);
                FixedParticles(0x376A, 20, 10, 5032, UniqueHue, 0, EffectLayer.Head);
                Hits = HitsMax;
                Mana = ManaMax;
                Stam = StamMax;
                return false; // Cancel death
            }
            return base.OnBeforeDeath();
        }

        // --- Movement Aura: Feline Reflex ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m == this || !Alive || !m.Alive || m.Map != Map || !m.InRange(Location, 2) || !CanBeHarmful(m, false))
                return;

            if (m is Mobile target)
            {
                // Slow the intruder briefly
                target.SendMessage(0x35, "The Vault Kitty's aura slows your footing!");
                target.FixedParticles(0x376A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
                target.PlaySound(0x208);
                target.Paralyze(TimeSpan.FromSeconds(1.5));
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Combatant == null || Map == null || Map == Map.Internal)
                return;

            DateTime now = DateTime.UtcNow;

            // Phase Pounce
            if (now >= m_NextPounceTime && this.InRange(Combatant.Location, 12))
            {
                PhasePounce();
                m_NextPounceTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }
            // Mana Surge AoE
            else if (now >= m_NextSurgeTime && this.InRange(Combatant.Location, 8))
            {
                ManaSurge();
                m_NextSurgeTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            }
            // Shimmer Decoy
            else if (now >= m_NextDecoyTime && this.InRange(Combatant.Location, 10))
            {
                ShimmerDecoy();
                m_NextDecoyTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
        }

        // --- Ability: Phase Pounce (teleport + heavy claw strike) ---
        public void PhasePounce()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*Shadow leap!*");
            PlaySound(0x20B);
			Point3D newLocation = new Point3D(target.X + Utility.RandomMinMax(-1,1), target.Y + Utility.RandomMinMax(-1,1), target.Z);
			if (Map.CanFit(newLocation, 16, false, false))
				MoveToWorld(newLocation, Map);


            DoHarmful(target);
            int dmg = Utility.RandomMinMax(50, 70);
            AOS.Damage(target, this, dmg, 100, 0, 0, 0, 0); // Pure physical claw
            target.FixedParticles(0x3779, 20, 10, 5032, UniqueHue, 0, EffectLayer.Waist);
        }

        // --- Ability: Mana Surge (AoE mana drain + PoisonTile hazard) ---
        public void ManaSurge()
        {
            Say("*Feel the drain!*");
            PlaySound(0x1F9);
            // Central effect
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x374A, 15, 30, UniqueHue, 0, 5032, 0);

            // Pull in nearby foes
            var targets = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && m is Mobile target && CanBeHarmful(target, false))
                    targets.Add(target);
            }

            foreach (Mobile victim in targets)
            {
                DoHarmful(victim);
                // Drain mana
                int drain = Utility.RandomMinMax(15, 30);
                if (victim.Mana >= drain)
                {
                    victim.Mana -= drain;
                    victim.SendMessage(0x22, "Your arcane reserves are siphoned!");
                    victim.FixedParticles(0x3728, 10, 12, 5032, UniqueHue, 0, EffectLayer.Head);
                    victim.PlaySound(0x1F8);
                }
                // Spawn poison hazard under them
                PoisonTile tile = new PoisonTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(victim.Location, Map);
            }
        }

        // --- Ability: Shimmer Decoy (spawn TrapWeb tiles for confusion) ---
        public void ShimmerDecoy()
        {
            Say("*Illusion— activate!*");
            PlaySound(0x208);

            for (int i = 0; i < 5; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;

                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                TrapWeb web = new TrapWeb();
                web.Hue = UniqueHue;
                web.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        // --- Unique Sounds ---
        public override int GetIdleSound()   { return 0x4FD; }
        public override int GetAngerSound()  { return 0x4FE; }
        public override int GetAttackSound() { return 0x4FC; }
        public override int GetHurtSound()   { return 0x4FF; }
        public override int GetDeathSound()  { return 0x4FB; }

        // --- Loot & Rewards ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 10));
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new SerpentsCry()); // Rare unique artifact
        }

        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 6;

        // --- Serialization ---
        public VaultKitty(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_LivesRemaining);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_LivesRemaining = reader.ReadInt();
            // Re-init timers
            m_NextPounceTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextSurgeTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextDecoyTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
        }
    }
}
