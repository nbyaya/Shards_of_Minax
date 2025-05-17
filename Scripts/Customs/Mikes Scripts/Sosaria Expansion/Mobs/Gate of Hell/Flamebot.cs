using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // For additional spell effects
using Server.Network; // For visual/sound effects
using System.Collections.Generic; // For lists used in AoE targeting
using Server.Spells.Fourth; // For fire field effects

namespace Server.Mobiles
{
    [CorpseName("a charred automaton remnant")]
    public class Flamebot : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextNovaTime;
        private DateTime m_NextOverloadTime;
        private DateTime m_NextChargeTime;
        private Point3D m_LastLocation;

        // Unique Hue for a fiery look (adjust as desired)
        private const int UniqueHue = 1175;

        [Constructable]
        public Flamebot() 
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "Flamebot";
            Body = 0x2F5;              // Same body as ExodusMinion
            BaseSoundID = 0x218;       // Re-use sound IDs from your sample
            Hue = UniqueHue;           // Give Flamebot its unique hue

            // --- Advanced Stats ---
            SetStr(1000, 1200);
            SetDex(150, 200);
            SetInt(300, 400);

            SetHits(2000, 2500);
            SetStam(300, 350);
            SetMana(400, 500);

            SetDamage(30, 40);

            // Primarily fire damage (with a bit of physical)
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Fire, 90);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 95, 100);
            SetResistance(ResistanceType.Cold, 0, 5);    // Extremely vulnerable to cold
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 50, 60);

            // --- Enhanced Skills ---
            SetSkill(SkillName.EvalInt, 100.1, 115.0);
            SetSkill(SkillName.Magery, 100.1, 115.0);
            SetSkill(SkillName.MagicResist, 110.2, 125.0);
            SetSkill(SkillName.Tactics, 100.1, 110.0);
            SetSkill(SkillName.Wrestling, 100.1, 110.0);
            SetSkill(SkillName.Anatomy, 80.0, 90.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 80;
            ControlSlots = 5;

            // --- Initialize ability cooldowns ---
            m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextOverloadTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_LastLocation = this.Location;

            // --- Standard Loot ---
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 8)));
        }

        // --- Sound Overrides (use ExodusMinion sounds) ---
        public override int GetIdleSound() { return 0x218; }
        public override int GetAngerSound() { return 0x26C; }
        public override int GetDeathSound() { return 0x211; }
        public override int GetAttackSound() { return 0x232; }
        public override int GetHurtSound() { return 0x140; }

        // --- Unique Ability: Flame Nova (AoE burst) ---
        public void FlameNovaAttack()
        {
            if (Map == null)
                return;

            // Play a bursting sound and visual effect on Flamebot
            PlaySound(0x208);
            FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 6); // 6-tile radius AoE

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    targets.Add(m);
                }
            }
            eable.Free();

            if (targets.Count > 0)
            {
                // Striking visual effect at Flamebot's location
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5029, 0);

                foreach (Mobile m in targets)
                {
                    DoHarmful(m);
                    int damage = Utility.RandomMinMax(50, 70); // Significant AoE damage (fire-only)
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Head);
                }
            }
        }

        // --- Unique Ability: Thermal Overload (shockwave when low on health) ---
        public void ThermalOverloadAttack()
        {
            if (Map == null)
                return;
            
            // Only trigger when Flamebot's health drops below 50%
            if (Hits < HitsMax / 2)
            {
                PlaySound(0x208);
                FixedParticles(0x3709, 20, 40, 5052, EffectLayer.Waist);

                List<Mobile> targets = new List<Mobile>();
                IPooledEnumerable eable = Map.GetMobilesInRange(Location, 8); // 8-tile radius

                foreach (Mobile m in eable)
                {
                    if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                        targets.Add(m);
                }
                eable.Free();

                foreach (Mobile m in targets)
                {
                    DoHarmful(m);
                    int damage = Utility.RandomMinMax(35, 50);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Head);
                }
            }
        }

        // --- Unique Ability: Flame Charge (dash attack with fire trail) ---
        public void FlameChargeAttack()
        {
            if (Map == null || Combatant == null)
                return;

            if (!(Combatant is Mobile target))
                return;

            // Dash toward the target by moving adjacent (with slight random offset)
            Point3D targetLocation = target.Location;
            Point3D newLocation = new Point3D(
                targetLocation.X + Utility.RandomMinMax(-1, 1),
                targetLocation.Y + Utility.RandomMinMax(-1, 1),
                targetLocation.Z);

            if (Map.CanFit(newLocation.X, newLocation.Y, newLocation.Z, 16, false, false))
            {
                // Store current location for the fire trail effect
                Point3D oldLocation = this.Location;
                this.Location = newLocation;
                PlaySound(0x160); // Dash sound effect

                // Leave behind a short-lived field of flame at the old location
                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(4 + Utility.Random(3));
                int damage = 2;
                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, Map, duration, damage);
                FixedParticles(0x3709, 10, 30, 5052, EffectLayer.CenterFeet);
            }
        }

        // --- Override Movement to leave a fire trail ---
        public override bool Move(Direction d)
        {
            bool moved = base.Move(d);

            if (moved && Combatant != null && Map != null)
            {
                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(4));
                int damage = 2;
                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, m_LastLocation, this, Map, duration, damage);
            }

            m_LastLocation = this.Location;
            return moved;
        }

        // --- Override OnThink to trigger abilities on cooldown ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal)
                return;

            if (Combatant is Mobile target)
            {
                if (DateTime.UtcNow >= m_NextNovaTime && InRange(target.Location, 6))
                {
                    FlameNovaAttack();
                    m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
                }
                if (DateTime.UtcNow >= m_NextOverloadTime && Hits < HitsMax / 2 && InRange(target.Location, 8))
                {
                    ThermalOverloadAttack();
                    m_NextOverloadTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
                }
                if (DateTime.UtcNow >= m_NextChargeTime && InRange(target.Location, 10))
                {
                    FlameChargeAttack();
                    m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
                }
            }
        }

        // --- Retaliation when struck by melee attacks ---
        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (attacker != null && attacker.Alive && attacker.Weapon is BaseRanged && Utility.RandomDouble() < 0.3)
            {
                if (attacker is Mobile target)
                {
                    MovingParticles(target, 0x379F, 7, 0, false, true, UniqueHue, 0, 0x211);
                    target.PlaySound(0x229);
                    DoHarmful(target);
                    AOS.Damage(target, this, 40, 0, 100, 0, 0, 0);
                }
            }
        }

        // --- Death Explosion: Spawns HotLavaTiles with dramatic effects ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                int lavaTilesToDrop = 10;
                List<Point3D> effectLocations = new List<Point3D>();

                for (int i = 0; i < lavaTilesToDrop; i++)
                {
                    int xOffset = Utility.RandomMinMax(-3, 3);
                    int yOffset = Utility.RandomMinMax(-3, 3);
                    if (xOffset == 0 && yOffset == 0 && i < lavaTilesToDrop - 1)
                        xOffset = Utility.RandomBool() ? 1 : -1;

                    Point3D lavaLocation = new Point3D(X + xOffset, Y + yOffset, Z);

                    if (!Map.CanFit(lavaLocation.X, lavaLocation.Y, lavaLocation.Z, 16, false, false))
                    {
                        lavaLocation.Z = Map.GetAverageZ(lavaLocation.X, lavaLocation.Y);
                        if (!Map.CanFit(lavaLocation.X, lavaLocation.Y, lavaLocation.Z, 16, false, false))
                            continue;
                    }

                    effectLocations.Add(lavaLocation);

                    HotLavaTile droppedLava = new HotLavaTile();
                    droppedLava.Hue = UniqueHue;
                    droppedLava.MoveToWorld(lavaLocation, Map);

                    Effects.SendLocationParticles(EffectItem.Create(lavaLocation, Map, EffectItem.DefaultDuration),
                        0x3709, 10, 20, UniqueHue, 0, 5016, 0);
                }

                Point3D deathLocation = Location;
                if (effectLocations.Count > 0)
                    deathLocation = effectLocations[Utility.Random(effectLocations.Count)];

                Effects.PlaySound(deathLocation, Map, 0x218);
                Effects.SendLocationParticles(EffectItem.Create(deathLocation, Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5052, 0);
            }
            base.OnDeath(c);
        }

        // --- Standard Properties & Loot Settings ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } }
        public override double DispelDifficulty { get { return 140.0; } }
        public override double DispelFocus { get { return 70.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls, 1);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 8));

            if (Utility.RandomDouble() < 0.02) // 2% chance for a rare cloak drop
                PackItem(new InfernosEmbraceCloak());

            if (Utility.RandomDouble() < 0.05) // 5% chance for a rare resource drop
                PackItem(new DaemonBone(Utility.RandomMinMax(3, 5)));
        }

        public Flamebot(Serial serial) : base(serial)
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
            m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextOverloadTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
        }
    }
}
