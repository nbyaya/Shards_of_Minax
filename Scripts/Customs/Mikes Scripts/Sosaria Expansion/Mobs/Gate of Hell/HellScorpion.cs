using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // For spell effects
using Server.Network; // For visual effects
using System.Collections.Generic;
using Server.Spells.Fourth; // For FireFieldSpell effects

namespace Server.Mobiles
{
    [CorpseName("a charred scorpion corpse")]
    public class HellScorpion : BaseCreature
    {
        // Timers for special abilities to prevent spamming
        private DateTime m_NextTrailTime;
        private DateTime m_NextNovaTime;
        private DateTime m_NextStingTime;
        private Point3D m_LastLocation;

        // Unique Hue for the Hell Scorpion (a fiery tint)
        private const int UniqueHue = 1350;

        [Constructable]
        public HellScorpion()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Hell Scorpion";
            Body = 48;         // Same as the base scorpion body
            BaseSoundID = 397; // Same as the base scorpion sound
            Hue = UniqueHue;

            // --- Enhanced Stats ---
            SetStr(300, 400);
            SetDex(150, 200);
            SetInt(80, 100);

            SetHits(800, 1000);
            SetStam(150, 200);
            SetMana(100, 150);

            SetDamage(15, 25);

            // --- Damage Types ---
            // Mix physical with predominantly fire damage
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Fire, 70);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 90, 100); // High fire resistance
            SetResistance(ResistanceType.Cold, 5, 10);     // Vulnerable to cold
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            // --- Skills ---
            SetSkill(SkillName.Poisoning, 90.0, 110.0); // Venomous by nature
            SetSkill(SkillName.Tactics, 90.0, 110.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);
            SetSkill(SkillName.MagicResist, 80.0, 100.0);

            Fame = 8000;
            Karma = -8000;
            VirtualArmor = 50;
            ControlSlots = 2;

            // --- Initialize Ability Timers ---
            m_NextTrailTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(3, 5));
            m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextStingTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8));

            m_LastLocation = this.Location;

            // --- Loot Setup ---
            PackItem(new SulfurousAsh(Utility.RandomMinMax(5, 10)));
            PackItem(new LesserPoisonPotion());
        }

        // --- Molten Trail Ability ---
        // As the Hell Scorpion moves, it leaves behind a fleeting trail of molten fire.
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            // Create a molten trail if the creature has moved and the timer allows
            if (this.Alive && this.Map != null && this.Location != m_LastLocation)
            {
                if (DateTime.UtcNow >= m_NextTrailTime)
                {
                    Point3D trailLocation = m_LastLocation;
                    m_LastLocation = this.Location;
                    int itemID = 0x398C; // Graphic for lava/fire field
                    TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(3));
                    int damage = 5; // Minor damage to those who step into it

                    var field = new FireFieldSpell.FireFieldItem(itemID, trailLocation, this, this.Map, duration, damage);
                    m_NextTrailTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(3, 6));
                }
            }
            base.OnMovement(m, oldLocation);
        }

        // --- Overridden OnThink for Special Attacks ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal)
                return;

            // Ensure Combatant is a Mobile before accessing Mobile-specific members
            if (!(Combatant is Mobile target))
                return;

            // --- Scorching Nova Attack (Area of Effect) ---
            if (DateTime.UtcNow >= m_NextNovaTime && this.InRange(target.Location, 6))
            {
                ScorchingNovaAttack();
                m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // --- Hellfire Sting Attack (Melee range special) ---
            else if (DateTime.UtcNow >= m_NextStingTime && this.InRange(target.Location, 1))
            {
                HellfireStingAttack(target);
                m_NextStingTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            }
        }

        // --- Unique Ability: Scorching Nova ---
        // Deals burst AoE fire damage to nearby enemies.
        public void ScorchingNovaAttack()
        {
            if (Map == null)
                return;

            this.PlaySound(0x208); // Fireball-like explosion sound

            // Send a visual explosion effect from the Hell Scorpion’s location.
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 40, UniqueHue, 0, 5029, 0);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 5);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    targets.Add(m);
                }
            }
            eable.Free();

            foreach (Mobile targ in targets)
            {
                DoHarmful(targ);
                int damage = Utility.RandomMinMax(30, 45);
                AOS.Damage(targ, this, damage, 0, 100, 0, 0, 0); // 100% fire damage
                targ.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head);
            }
        }

        // --- Unique Ability: Hellfire Sting ---
        // A melee-based, fiery tail sting that not only deals immediate damage but also inflicts a burning effect.
        public void HellfireStingAttack(Mobile target)
        {
            if (target == null || Map == null || !target.Alive)
                return;

            this.PlaySound(0x0F7); // Choose an appropriate sting sound effect
            target.FixedParticles(0x36BD, 10, 15, 5033, UniqueHue, 0, EffectLayer.Head);
            DoHarmful(target);
            int damage = Utility.RandomMinMax(20, 30);
            AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);

            // Apply a burning effect: over a few seconds, the target takes additional fire damage.
            Timer.DelayCall(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0), 3, () =>
            {
                if (target != null && target.Alive)
                {
                    AOS.Damage(target, this, Utility.RandomMinMax(5, 10), 0, 100, 0, 0, 0);
                    target.SendMessage("The burning venom scorches your flesh!");
                }
            });
        }

        // --- Death Explosion ---
        // Upon death, the Hell Scorpion bursts into a short-range inferno, leaving behind several hot lava tiles.
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            int lavaTilesToDrop = 8;
            List<Point3D> effectLocations = new List<Point3D>();

            for (int i = 0; i < lavaTilesToDrop; i++)
            {
                int xOffset = Utility.RandomMinMax(-2, 2);
                int yOffset = Utility.RandomMinMax(-2, 2);
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

                Effects.SendLocationParticles(EffectItem.Create(lavaLocation, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 20, UniqueHue, 0, 5016, 0);
            }

            Point3D deathLocation = this.Location;
            if (effectLocations.Count > 0)
                deathLocation = effectLocations[Utility.Random(effectLocations.Count)];

            Effects.PlaySound(deathLocation, this.Map, 0x218); // Large explosion sound
            Effects.SendLocationParticles(EffectItem.Create(deathLocation, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            base.OnDeath(c);
        }

        // --- Standard Overrides ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 3; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Average, 2);
        }

        public HellScorpion(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_NextTrailTime);
            writer.Write(m_NextNovaTime);
            writer.Write(m_NextStingTime);
            writer.Write(m_LastLocation);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextTrailTime = reader.ReadDateTime();
            m_NextNovaTime = reader.ReadDateTime();
            m_NextStingTime = reader.ReadDateTime();
            m_LastLocation = reader.ReadPoint3D();
        }
    }
}
