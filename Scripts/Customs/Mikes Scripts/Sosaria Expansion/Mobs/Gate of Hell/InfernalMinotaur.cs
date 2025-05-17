using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;           // For potential spell effects
using Server.Network;          // For visual/sound effects
using System.Collections.Generic;
using Server.Spells.Fourth;    // For fire field effects

namespace Server.Mobiles
{
    [CorpseName("an infernal minotaur corpse")]
    public class InfernalMinotaur : BaseCreature
    {
        // Ability cooldown timers and tracking of last location for environmental effects
        private DateTime m_NextAuraTime;
        private DateTime m_NextChargeTime;
        private DateTime m_NextBreathTime;
        private Point3D m_LastLocation;

        // Unique hue for the fire theme (adjust as desired)
        private const int UniqueHue = 1161;

        [Constructable]
        public InfernalMinotaur() 
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Infernal Minotaur";
            Body = 263; // Same base body as standard Minotaur
            BaseSoundID = 0x597; // Uses similar sound base
            Hue = UniqueHue;


            // --- Boosted Stats ---
            SetStr(550, 600);
            SetDex(120, 140);
            SetInt(60, 80);

            SetHits(1500, 1700);

            SetDamage(20, 30);
            // Damage is 60% physical, 40% fire damage
            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Fire, 40);

            // --- Adjusted Resistances ---
            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 90, 100);
            SetResistance(ResistanceType.Cold, 20, 30); // Some vulnerability can be added here if desired
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            // --- Enhanced Skills ---
            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 50;
            ControlSlots = 5;

            // --- Initialize Ability Cooldowns ---
            m_NextAuraTime = DateTime.UtcNow + TimeSpan.FromSeconds(5);
            m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            m_LastLocation = this.Location;

            // Standard loot plus extra ingredients
            PackItem(new SulfurousAsh(Utility.RandomMinMax(8, 12)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(4, 8)));

            // Set a basic weapon ability for flavor
            SetWeaponAbility(WeaponAbility.ParalyzingBlow);
        }

        public override int TreasureMapLevel { get { return 4; } }

        // --- Sound Overrides (using base minotaur sound values) ---
        public override int GetAngerSound() { return 0x597; }
        public override int GetIdleSound() { return 0x596; }
        public override int GetAttackSound() { return 0x599; }
        public override int GetHurtSound() { return 0x59a; }
        public override int GetDeathSound() { return 0x59c; }

        // --- Unique Ability: Infernal Aura ---
        // As mobiles move near the minotaur, leave a burning fire field that damages them.
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.Alive && this.Alive)
            {
                if (DateTime.UtcNow >= m_NextAuraTime && m.InRange(this.Location, 2))
                {
                    int itemID = 0x398C; // Fire field tile appearance
                    TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(4));
                    int damage = 10;

                    // Spawn the fire field at the mobile's old location
                    var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);

                    // Create visual effect and sound
                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.CenterFeet);
                    m.PlaySound(0x208);

                    if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, Utility.RandomMinMax(8, 15), 0, 100, 0, 0, 0);
                    }
                    m_NextAuraTime = DateTime.UtcNow + TimeSpan.FromSeconds(5);
                }
            }
            base.OnMovement(m, oldLocation);
        }

        // --- OnThink Override ---
        // In addition to standard thinking, check for special attack conditions.
        public override void OnThink()
        {
            base.OnThink();

            // Create environmental fire fields along the path (similar to leaving a trail)
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal)
            {
                Point3D oldLocation = m_LastLocation;
                m_LastLocation = this.Location;

                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(8 + Utility.Random(5));
                int damage = 5;

                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);
            }

            if (Combatant == null || Map == null || Map == Map.Internal)
                return;

            // Always check that Combatant is a Mobile before accessing mobile members
            if (!(Combatant is Mobile target))
                return;

            // --- Fiery Charge Attack ---
            // If the target is within 12 tiles and the cooldown has expired, charge the target.
            if (DateTime.UtcNow >= m_NextChargeTime && this.InRange(target.Location, 12))
            {
                FieryChargeAttack(target);
                m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            }
            // --- Flame Breath Attack ---
            // If the target is close (within 8 tiles) and the cooldown has expired, unleash a cone of fire.
            else if (DateTime.UtcNow >= m_NextBreathTime && this.InRange(target.Location, 8))
            {
                FlameBreathAttack();
                m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(25);
            }
        }

        // --- Fiery Charge Attack --- 
        // Charges toward the target to deliver a powerful melee strike and an area explosion.
        public void FieryChargeAttack(Mobile target)
        {
            if (target == null || Map == null)
                return;

            this.Say("*The Infernal Minotaur charges with burning fury!*");
            PlaySound(0x160); // Charge sound effect

            // Face the target and simulate a charge (you might adjust movement behavior as needed)
            Direction = GetDirectionTo(target);
            // Optionally, you can call a movement routine to quickly close distance
            // For example: Move(this, target, 5, false);

            // Create an explosion effect at the target’s location
            Effects.SendLocationParticles(
                EffectItem.Create(target.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 50, UniqueHue, 0, 5052, 0);
            target.PlaySound(0x208);

            // Direct damage on impact
            DoHarmful(target);
            AOS.Damage(target, this, Utility.RandomMinMax(40, 60), 0, 100, 0, 0, 0);

            // Also affect any nearby mobiles with a smaller burst
            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(target.Location, 2);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            foreach (Mobile aoe in targets)
            {
                DoHarmful(aoe);
                AOS.Damage(aoe, this, Utility.RandomMinMax(20, 30), 0, 100, 0, 0, 0);
                aoe.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Head);
            }
        }

        // --- Flame Breath Attack ---
        // Unleashes a cone of fire toward the target; mobiles in a 90° arc in front of the minotaur are affected.
        public void FlameBreathAttack()
        {
            if (Combatant == null || Map == null)
                return;

            if (!(Combatant is Mobile target))
                return;

            this.Say("*The Infernal Minotaur exhales a torrent of flame!*");
            PlaySound(0x208); // Breath sound

            // Display a cone-shaped visual effect at the minotaur's location
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 15, 50, UniqueHue, 0, 5052, 0);

            // Collect targets in a 6-tile radius; only those roughly in front (within a 90° cone) are affected
            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    if (InCone(target, m))
                        targets.Add(m);
                }
            }
            eable.Free();

            foreach (Mobile m in targets)
            {
                DoHarmful(m);
                int damage = Utility.RandomMinMax(25, 35);
                AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                m.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Head);
            }
        }

        // --- Helper Method --- 
        // Checks if mobile 'm' is within a roughly 90-degree cone in front of the target direction.
        public bool InCone(Mobile target, Mobile m)
        {
			int ourDir = (int)GetDirectionTo(target);
			int dirToM = (int)GetDirectionTo(m);
            int diff = Math.Abs(ourDir - dirToM);
            // Directions in Ultima Online use a scale 0-15; an arc of ~90° is about 4 increments to each side (8 total)
            return (diff <= 4 || diff >= 12);
        }

        // --- Death Explosion ---
        // On death, the Infernal Minotaur releases a fiery explosion that drops burning lava tiles.
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            int lavaTilesToDrop = 10;
            List<Point3D> effectLocations = new List<Point3D>();

            for (int i = 0; i < lavaTilesToDrop; i++)
            {
                int xOffset = Utility.RandomMinMax(-3, 3);
                int yOffset = Utility.RandomMinMax(-3, 3);
                if (xOffset == 0 && yOffset == 0 && i < lavaTilesToDrop - 1)
                    xOffset = Utility.RandomBool() ? 1 : -1;

                Point3D lavaLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);
                if (!Map.CanFit(lavaLocation.X, lavaLocation.Y, lavaLocation.Z, 16, false, false))
                {
                    lavaLocation.Z = Map.GetAverageZ(lavaLocation.X, lavaLocation.Y);
                    if (!Map.CanFit(lavaLocation.X, lavaLocation.Y, lavaLocation.Z, 16, false, false))
                        continue;
                }
                effectLocations.Add(lavaLocation);

                HotLavaTile droppedLava = new HotLavaTile();
                droppedLava.Hue = UniqueHue;
                droppedLava.MoveToWorld(lavaLocation, this.Map);

                Effects.SendLocationParticles(
                    EffectItem.Create(lavaLocation, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 20, UniqueHue, 0, 5016, 0);
            }

            Point3D deathLocation = this.Location;
            if (effectLocations.Count > 0)
                deathLocation = effectLocations[Utility.Random(effectLocations.Count)];

            Effects.PlaySound(deathLocation, this.Map, 0x218);
            Effects.SendLocationParticles(
                EffectItem.Create(deathLocation, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } }
        public override double DispelDifficulty { get { return 140.0; } }
        public override double DispelFocus { get { return 65.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 8));

            // 2% chance for a special drop
            if (Utility.RandomDouble() < 0.02)
            {
                PackItem(new InfernosEmbraceCloak());
            }
        }

        public InfernalMinotaur(Serial serial) 
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

            // Re-initialize cooldowns on load
            m_NextAuraTime = DateTime.UtcNow;
            m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }
    }
}
