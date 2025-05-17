using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;      // For spell effects (e.g., fire fields)
using Server.Network;     // For visual effects
using System.Collections.Generic; // For list handling in AoE targeting

namespace Server.Mobiles
{
    [CorpseName("a Scorching Webspinner corpse")]
    public class ScorchingWebspinner : BaseCreature
    {
        // Timers for special abilities to prevent spamming
        private DateTime m_NextAuraTime;
        private DateTime m_NextNovaTime;
        private DateTime m_NextWebLashTime;

        // Unique Hue for our fire-themed boss (fire orange/red)
        private const int UniqueHue = 1161;

        [Constructable]
        public ScorchingWebspinner() 
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Scorching Webspinner";
            Body = 20; // Using the same body as the Frost Spider
            BaseSoundID = 0x388; // Same base sound
            Hue = UniqueHue;

            // --- Advanced Stats ---
            SetStr(300, 350);
            SetDex(200, 240);
            SetInt(100, 150);

            SetHits(600, 800);
            SetStam(200, 300);
            SetMana(150, 200);

            SetDamage(20, 30);

            // Damage types: Mostly fire with a bit of physical
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 80);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, -10, 0); // Vulnerable to cold
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 30, 40);

            // --- Skills ---
            SetSkill(SkillName.MagicResist, 80.1, 100.0);
            SetSkill(SkillName.Tactics, 80.1, 100.0);
            SetSkill(SkillName.Wrestling, 80.1, 100.0);

            Fame = 12000;
            Karma = -12000;

            VirtualArmor = 40;
            ControlSlots = 3;
            Tamable = true; // This is a boss-level creature

            // --- Initialize Ability Cooldowns ---
            m_NextAuraTime = DateTime.UtcNow;
            m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextWebLashTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));

            // --- Standard Loot (customize as desired) ---
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 8)));
        }

        // --- Unique Passive Ability: Flame Aura ---
        // When any mobile moves close to the Webspinner, it leaves behind a brief fire field.
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.Alive && this.Alive && DateTime.UtcNow >= m_NextAuraTime)
            {
                if (m.InRange(this.Location, 2))
                {
                    // Create a brief fire field effect at the mobile's old location
                    int itemID = 0x398C;
                    TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(4));
                    int damage = 3;

                    var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);

                    // Visual and sound effects (using Flamestrike-style particles)
                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.CenterFeet);
                    m.PlaySound(0x208);

                    // Deal fire damage if the target can be harmed
                    if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, Utility.RandomMinMax(10, 15), 0, 100, 0, 0, 0);
                    }

                    // Set aura cooldown (e.g., 3 seconds)
                    m_NextAuraTime = DateTime.UtcNow + TimeSpan.FromSeconds(3);
                }
            }
            base.OnMovement(m, oldLocation);
        }

        // --- Core AI Thinking: Checks for ability use ---
        public override void OnThink()
        {
            base.OnThink();

            // Ensure we have a valid mobile combatant
            if (Combatant is Mobile target && target != null && Map != null && Map != Map.Internal)
            {
                // Flame Nova: AoE burst when a target is within 8 tiles
                if (DateTime.UtcNow >= m_NextNovaTime && this.InRange(target.Location, 8))
                {
                    FlameNovaAttack();
                    m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
                }
                // Web Lash: Targeted burning web attack when within 3 tiles
                else if (DateTime.UtcNow >= m_NextWebLashTime && this.InRange(target.Location, 3))
                {
                    WebLashAttack(target);
                    m_NextWebLashTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
                }
            }
        }

        // --- Unique Ability: Flame Nova Attack ---
        // Releases a burst of fire in a 6-tile radius, damaging all nearby valid targets.
        public void FlameNovaAttack()
        {
            if (Map == null)
                return;

            PlaySound(0x208);
            FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6);

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            if (targets.Count > 0)
            {
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5029, 0);
                foreach (Mobile t in targets)
                {
                    DoHarmful(t);
                    int damage = Utility.RandomMinMax(30, 45);
                    AOS.Damage(t, this, damage, 0, 100, 0, 0, 0);
                    t.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Head);
                }
            }
        }

        // --- Unique Ability: Web Lash Attack ---
        // The Scorching Webspinner lashes out with a burning web that deals immediate damage and
        // then applies a delayed burn effect (simulated here with several delayed damage ticks).
        public void WebLashAttack(Mobile target)
        {
            if (target == null || Map == null)
                return;

            // Play an attack sound (choose an appropriate sound ID)
            PlaySound(0x164);

            // Send a moving particle effect to simulate the web projectile
            Effects.SendMovingParticles(
                new Entity(Serial.Zero, this.Location, this.Map),
                new Entity(Serial.Zero, target.Location, this.Map),
                0x36D4, 5, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0);

            DoHarmful(target);
            int damage = Utility.RandomMinMax(15, 25);
            AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);

            // Simulate a burning effect: 3 ticks of additional fire damage over 3 seconds
            for (int i = 1; i <= 3; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i), () =>
                {
                    if (target == null || !target.Alive || Map == null)
                        return;
                    if (CanBeHarmful(target, false) && SpellHelper.ValidIndirectTarget(this, target))
                    {
                        AOS.Damage(target, this, 5, 0, 100, 0, 0, 0);
                        target.FixedParticles(0x36BD, 10, 10, 5036, UniqueHue, 0, EffectLayer.Waist);
                    }
                });
            }
        }

        // --- Death Effect: Burning Web Explosion ---
        // When slain, the Scorching Webspinner leaves behind burning web tiles that create a final explosion.
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                int webTilesToDrop = 8;
                List<Point3D> effectLocations = new List<Point3D>();

                for (int i = 0; i < webTilesToDrop; i++)
                {
                    int xOffset = Utility.RandomMinMax(-3, 3);
                    int yOffset = Utility.RandomMinMax(-3, 3);
                    if (xOffset == 0 && yOffset == 0 && i < webTilesToDrop - 1)
                        xOffset = Utility.RandomBool() ? 1 : -1;

                    Point3D webLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);
                    if (!Map.CanFit(webLocation.X, webLocation.Y, webLocation.Z, 16, false, false))
                    {
                        webLocation.Z = Map.GetAverageZ(webLocation.X, webLocation.Y);
                        if (!Map.CanFit(webLocation.X, webLocation.Y, webLocation.Z, 16, false, false))
                            continue;
                    }

                    effectLocations.Add(webLocation);

                    // Spawn a burning web tile (assumes TrapWeb is defined elsewhere)
                    TrapWeb webTile = new TrapWeb();
                    webTile.Hue = UniqueHue;
                    webTile.MoveToWorld(webLocation, Map);

                    Effects.SendLocationParticles(EffectItem.Create(webLocation, Map, EffectItem.DefaultDuration), 0x3709, 10, 20, UniqueHue, 0, 5016, 0);
                }

                // Create a big explosion effect at one of the generated points
                Point3D deathLocation = this.Location;
                if (effectLocations.Count > 0)
                    deathLocation = effectLocations[Utility.Random(effectLocations.Count)];

                Effects.PlaySound(deathLocation, Map, 0x218);
                Effects.SendLocationParticles(EffectItem.Create(deathLocation, Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0);
            }

            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } }
        public override double DispelDifficulty { get { return 125.0; } }
        public override double DispelFocus { get { return 50.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
            AddLoot(LootPack.MedScrolls, 1);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(3, 6));

            // Rare drops chance
            if (Utility.RandomDouble() < 0.02)
                PackItem(new MaxxiaScroll());
            if (Utility.RandomDouble() < 0.05)
                PackItem(new MaxxiaScroll(Utility.RandomMinMax(2, 4)));
        }

        public ScorchingWebspinner(Serial serial)
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

            m_NextAuraTime = DateTime.UtcNow;
            m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextWebLashTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
        }
    }
}
