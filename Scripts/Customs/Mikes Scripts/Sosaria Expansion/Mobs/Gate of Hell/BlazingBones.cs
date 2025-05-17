using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // Needed for spell effects
using Server.Network; // Needed for effects
using System.Collections.Generic; // Needed for lists in AoE targeting
using Server.Spells.Fourth; // Needed for FireFieldSpell

namespace Server.Mobiles
{
    [CorpseName("a pile of blazing bones")] // Themed corpse name
    public class BlazingBones : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextBreathTime;
        private DateTime m_NextShrapnelTime;
        private DateTime m_NextFireTrailCheck; // Timer for leaving fire trails passively
        private Point3D m_LastLocation; // To track movement for fire trail

        // Unique Hue - Example: 1158 is a deep fiery orange. Adjust as desired.
        private const int UniqueHue = 1158;

        [Constructable]
        public BlazingBones() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.15, 0.3) // Slightly faster reaction than base knight
        {
            Name = "a Blazing Bones";
            Body = 57; // Bone Knight body
            BaseSoundID = 451; // Bone Knight sound
            Hue = UniqueHue;
 // Make sure it fits into the Gate of Hell faction if applicable

            // --- Significantly Boosted Stats ---
            SetStr(380, 480);
            SetDex(150, 200); // More agile than a standard knight
            SetInt(320, 420); // Needs Int for mana and ability scaling

            SetHits(900, 1200); // Tougher health pool
            SetStam(150, 200); // For movement/abilities
            SetMana(320, 420); // Mana pool for abilities

            SetDamage(19, 26); // Higher base damage

            // Primarily Fire damage, some physical from bones/impact
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Fire, 70);

            // --- Adjusted Resistances ---
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 75, 90); // Very high fire resist
            SetResistance(ResistanceType.Cold, 0, 10);   // Vulnerable to Cold
            SetResistance(ResistanceType.Poison, 40, 50); // Undead resistance
            SetResistance(ResistanceType.Energy, 40, 50);

            // --- Enhanced Skills ---
            SetSkill(SkillName.MagicResist, 105.1, 120.0);
            SetSkill(SkillName.Tactics, 100.1, 115.0);
            SetSkill(SkillName.Wrestling, 100.1, 115.0); // Using wrestling instead of packing a weapon that might burn up
            SetSkill(SkillName.Anatomy, 90.1, 100.0);  // Helps with damage output
            SetSkill(SkillName.Magery, 95.1, 110.0);   // For ability logic/scaling
            SetSkill(SkillName.EvalInt, 95.1, 110.0); // For ability logic/scaling

            Fame = 16000;
            Karma = -16000;

            VirtualArmor = 65; // Higher passive defense
            ControlSlots = 4; // Difficult to control
			Tamable = true;

            // Initialize ability cooldowns
            m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 15));
            m_NextShrapnelTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
            m_NextFireTrailCheck = DateTime.UtcNow + TimeSpan.FromSeconds(1.0); // Check frequently
            m_LastLocation = this.Location;

            // Pack fire-themed reagents or resources
            PackItem(new SulfurousAsh(Utility.RandomMinMax(6, 12)));
            PackItem(new DaemonBone(Utility.RandomMinMax(1, 3))); // Fits the theme

            // Add a faint, flickering light source
            AddItem(new Torch()); // Torches often have a nice flickering effect
            if (FindItemOnLayer(Layer.TwoHanded) is Torch t) t.Hue = UniqueHue; // Hue the torch
        }

        // --- Unique Ability: Trail of Embers ---
        // Check periodically in OnThink if moved, leave fire field behind
        private void CheckLeaveFireTrail()
        {
            if (this.Map == null || this.Map == Map.Internal)
                return;

            if (this.Location != m_LastLocation)
            {
                Point3D oldLocation = m_LastLocation;
                m_LastLocation = this.Location;

                // Only leave trail sometimes to avoid spamming fields everywhere
                if (Utility.RandomDouble() < 0.4) // 40% chance on movement check
                {
                    int itemID = Utility.RandomList(0x398C, 0x3996); // Fire field item IDs
                    TimeSpan duration = TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8));
                    int damagePerSecond = Utility.RandomMinMax(3, 5); // Lower damage than direct attacks

                    // Use FireFieldSpell's item for consistency if available
                    var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damagePerSecond);
                    field.Hue = UniqueHue; // Match the creature's hue
                }
            }
        }

        // --- Thinking Process for Special Attacks ---
        public override void OnThink()
        {
            base.OnThink();

            // Passive Fire Trail Check
            if (DateTime.UtcNow >= m_NextFireTrailCheck)
            {
                CheckLeaveFireTrail();
                m_NextFireTrailCheck = DateTime.UtcNow + TimeSpan.FromSeconds(0.5); // Check every half second if moved
            }

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Check if Combatant is a Mobile before using Mobile-specific properties/methods
            Mobile target = Combatant as Mobile;
            if (target == null) return; // Exit if Combatant isn't a Mobile

            // Prioritize abilities based on cooldown and range
            if (DateTime.UtcNow >= m_NextShrapnelTime && this.InRange(target.Location, 8) && Utility.RandomDouble() < 0.25) // 25% chance if in range and ready
            {
                BoneShrapnelAttack();
                m_NextShrapnelTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
                m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10)); // Delay breath after shrapnel
            }
            else if (DateTime.UtcNow >= m_NextBreathTime && this.InRange(target.Location, 6) && Utility.RandomDouble() < 0.40) // 40% chance if in range and ready
            {
                FireBreathAttack(target);
                m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 18));
            }
        }


        // --- Unique Ability: Bone Shrapnel Explosion ---
        public void BoneShrapnelAttack()
        {
            if (Map == null) return;

            this.PlaySound(0x56F); // A sharper, cracking sound (adjust if needed)
            this.Say("*Clack* KABOOM!"); // Optional flavor text

            // Visual effect: Explosion from self
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 40, UniqueHue, 0, 5052, 0); // Fiery explosion effect
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x36B0, 10, 40, UniqueHue, 0, 5052, 0); // Bone fragments effect (adjust ID if needed)

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6); // 6 tile radius AoE

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
                foreach (Mobile victim in targets)
                {
                    DoHarmful(victim);
                    int damage = Utility.RandomMinMax(45, 65); // Significant AoE damage
                    // Deal mixed damage: 40% physical (shrapnel), 60% fire
                    AOS.Damage(victim, this, damage, 40, 60, 0, 0, 0);

                    // Add a visual effect on the target
                    victim.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Head); // Fiery hit effect
                    victim.PlaySound(0x208); // Fire hit sound

                    // Chance to inflict a short bleed (minor physical aspect)
                    if (Utility.RandomDouble() < 0.15) // 15% chance
                    {
                         victim.ApplyPoison(this, Poison.Regular); // Representing a minor bleed/injury from shrapnel
                         victim.SendMessage("Fiery bone shards tear into your flesh!"); // Or use BleedAttack.BeginBleed(victim, this); if you have access/want real bleed
                    }
                }
            }
        }


        // --- Unique Ability: Searing Fire Breath ---
        public void FireBreathAttack(Mobile target)
        {
            if (target == null || Map == null || !Alive) return;

            this.MovingEffect(target, 0x36D4, 10, 0, false, false, UniqueHue, 0); // Dragon breath visual
            this.PlaySound(0x227); // Dragon breath sound
            this.Say("*Hsssss... FWOOSH!*");

            // Delay damage slightly to match visual effect arrival
            Timer.DelayCall(TimeSpan.FromSeconds(0.8), () =>
            {
                if (!this.Alive || this.Map == null) return; // Check validity inside timer

                Mobile directTarget = target; // Re-check target within timer scope
                if (directTarget == null || directTarget.Deleted || !directTarget.Alive || !CanBeHarmful(directTarget)) return;


                // Deal damage to the primary target
                int directDamage = Utility.RandomMinMax(50, 75);
                 DoHarmful(directTarget);
                AOS.Damage(directTarget, this, directDamage, 10, 90, 0, 0, 0); // Mostly fire damage
                directTarget.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.CenterFeet); // Impact effect


                // Cone effect - damage targets behind the primary target
                Point3D location = directTarget.Location;
                Map map = directTarget.Map;
                Direction dir = Utility.GetDirection(this, directTarget);

                for (int i = 1; i <= 3; i++) // Check 3 tiles behind the target
                {
                    int x = location.X;
                    int y = location.Y;
                    Movement.Movement.Offset(dir, ref x, ref y); // Calculate next tile in the direction

                    IPooledEnumerable coneTargets = map.GetMobilesInRange(new Point3D(x, y, location.Z), 0); // Check only that specific tile
                    foreach (Mobile coneTarget in coneTargets)
                    {
                        if (coneTarget != this && coneTarget != directTarget && CanBeHarmful(coneTarget, false) && SpellHelper.ValidIndirectTarget(this, coneTarget))
                        {
                            DoHarmful(coneTarget);
                            int coneDamage = Utility.RandomMinMax(30, 45); // Less damage than direct hit
                            AOS.Damage(coneTarget, this, coneDamage, 10, 90, 0, 0, 0);
                            coneTarget.FixedParticles(0x3709, 10, 20, 5052, EffectLayer.CenterFeet); // Smaller impact effect
                        }
                    }
                    coneTargets.Free();
                    location = new Point3D(x, y, location.Z); // Update location for next step in cone
                }
            });
        }


        // --- Death Explosion ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            // Big final explosion effect
            Effects.PlaySound(this.Location, this.Map, 0x208); // Explosion sound
            Effects.PlaySound(this.Location, this.Map, 0x56F); // Bone crack sound
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0); // Large fiery explosion
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x36B0, 10, 60, UniqueHue, 0, 5052, 0); // Large bone explosion


            // Damage nearby players in a radius
            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 5); // 5 tile radius death burst

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
                foreach (Mobile victim in targets)
                {
                     // Use this (even though dead) as the damager for kill credit etc.
                    int damage = Utility.RandomMinMax(60, 90); // High death burst damage
                    AOS.Damage(victim, this, damage, 30, 70, 0, 0, 0); // Mixed phys/fire
                    victim.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Head); // Fiery hit
                    victim.SendMessage("The Blazing Bones explodes in a shower of fire and shrapnel!");
                }
            }

            // Chance to leave some persistent fire fields
            for (int i = 0; i < Utility.RandomMinMax(3, 6); i++)
            {
                Point3D fieldLoc = new Point3D(
                    this.X + Utility.RandomMinMax(-3, 3),
                    this.Y + Utility.RandomMinMax(-3, 3),
                    this.Z);

                if (!Map.CanFit(fieldLoc.X, fieldLoc.Y, fieldLoc.Z, 16, false, false))
                {
                    fieldLoc.Z = Map.GetAverageZ(fieldLoc.X, fieldLoc.Y);
                     if (!Map.CanFit(fieldLoc.X, fieldLoc.Y, fieldLoc.Z, 16, false, false))
                        continue;
                }

                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(Utility.RandomList(0x398C, 0x3996), fieldLoc, this, this.Map, TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20)), Utility.RandomMinMax(5,8));
                field.Hue = UniqueHue;
            }


            base.OnDeath(c); // Generate loot etc.
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } } // Undead are immune to bleed
        public override TribeType Tribe { get { return TribeType.Undead; } }
        public override OppositionGroup OppositionGroup { get { return OppositionGroup.FeyAndUndead; } }
        public override Poison PoisonImmune { get { return Poison.Deadly; } } // Immune to poison

        // Increased Dispel difficulty
        public override double DispelDifficulty { get { return 125.0; } }
        public override double DispelFocus { get { return 45.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2); // Better base loot
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls, Utility.RandomBool() ? 1 : 2); // Good chance for scrolls
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 10)); // More gems

            // Chance for a unique drop
            if (Utility.RandomDouble() < 0.02) // 2% chance
            {
                // Example Unique Drop: Could be an artifact, special reagent, or cosmetic
                switch (Utility.Random(2))
                {
                     case 0: PackItem(new BlazingBoneShard(Utility.RandomMinMax(1, 3))); break; // A special reagent?
                     case 1: PackItem(new ObsidianSkullHelm()); break; // A themed rare item
                }
            }
        }

        // --- Serialization ---
        public BlazingBones(Serial serial) : base(serial)
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

            // Re-initialize cooldowns on load/restart
            m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextShrapnelTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextFireTrailCheck = DateTime.UtcNow + TimeSpan.FromSeconds(1.0);
            m_LastLocation = this.Location; // Initialize last location


            // Ensure torch is hued correctly on deserialize if it still exists
             if (FindItemOnLayer(Layer.TwoHanded) is Torch t) t.Hue = UniqueHue;
        }
    }

    // --- Example Unique Drops (Define these classes elsewhere in your Items folder) ---

    // Example Reagent Item
    public class BlazingBoneShard : Item
    {
        [Constructable]
        public BlazingBoneShard() : this(1) { }

        [Constructable]
        public BlazingBoneShard(int amount) : base(0xF7E) // Bone shard graphic
        {
            Name = "Blazing Bone Shard";
            Hue = 1158; // Match the creature's hue
            Stackable = true;
            Amount = amount;
            Weight = 0.1;
        }

        public BlazingBoneShard(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }

    // Example Armor Item
    public class ObsidianSkullHelm : BoneHelm // Or PlateHelm, etc.
    {
        public override int BaseFireResistance { get { return 10; } } // Add some fire resist

        [Constructable]
        public ObsidianSkullHelm() : base()
        {
            Name = "Obsidian Skull Helm";
            Hue = 1175; // Black or dark grey
            Attributes.BonusStr = 5;
            Attributes.RegenHits = 1;
            Attributes.LowerManaCost = 5;
        }

        public ObsidianSkullHelm(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }
}