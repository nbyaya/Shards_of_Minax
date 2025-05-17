using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // For potential spell effects
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an arcane shattered corpse")]
    public class SpellClaw : BaseCreature
    {
        // Ability timers
        private DateTime m_NextSiphonTime;
        private DateTime m_NextGlyphTime;
        private DateTime m_NextTeleportTime;

        // Unique hue for Spell Claw (e.g., a deep arcane magenta)
        private const int UniqueHue = 1350;

        [Constructable]
        public SpellClaw() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "Spell Claw";
            Title = "the Arcane Predator";
            // Using the TigersClaw-inspired body and sound
            Body = 249; // Example body ID for a bestial humanoid
            BaseSoundID = 0x3E3; // A feral, claw-like roar

            Hue = UniqueHue;

            // Enhanced stats for a powerful magic-themed foe
            SetStr(600, 650);
            SetDex(500, 550);
            SetInt(400, 450);

            SetHits(1200, 1300);
            SetStam(500, 550);
            SetMana(600, 700);

            // Damage: moderate base; split 50% Physical and 50% Energy
            SetDamage(20, 25);
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 70, 80);

            // Skills emphasizing magical prowess and combat ability
            SetSkill(SkillName.EvalInt, 110.0, 125.0);
            SetSkill(SkillName.Magery, 110.0, 125.0);
            SetSkill(SkillName.MagicResist, 115.0, 130.0);
            SetSkill(SkillName.Meditation, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);

            Fame = 30000;
            Karma = -30000;
            VirtualArmor = 80;
            ControlSlots = 5;

            // Initialize ability cooldowns (in seconds)
            m_NextSiphonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextGlyphTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextTeleportTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 35));

            // Basic loot: magic reagents and a chance at a unique magical drop
            PackItem(new BlackPearl(Utility.RandomMinMax(10, 15)));
            PackItem(new Nightshade(Utility.RandomMinMax(10, 15)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(10, 15)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            int dist = (int)GetDistanceToSqrt(Combatant.Location);

            // In melee range: attempt Mana Siphon Slash
            if (dist <= 2 && DateTime.UtcNow >= m_NextSiphonTime)
            {
                ManaSiphonSlash();
                m_NextSiphonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            // In moderate range: cast a Glyph of Binding
            else if (dist > 2 && dist <= 10 && DateTime.UtcNow >= m_NextGlyphTime)
            {
                GlyphOfBinding();
                m_NextGlyphTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }

            // If injured (less than 50% health) and off cooldown, try to teleport behind the enemy for a sudden strike
            if (Hits < (HitsMax * 0.5) && DateTime.UtcNow >= m_NextTeleportTime)
            {
                if (Combatant is Mobile target)
                {
                    TeleportClaw(target);
                    m_NextTeleportTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
                }
            }
        }

        // Mana Siphon Slash – a melee attack that both damages and drains the target's mana to heal Spell Claw.
        public void ManaSiphonSlash()
        {
            if (Combatant == null || Map == null)
                return;

            if (Combatant is Mobile target && SpellHelper.ValidIndirectTarget(this, target) && CanBeHarmful(target, false))
            {
                DoHarmful(target);

                // Play the claw-swipe sound and show a particle effect
                PlaySound(0x3E3);
                FixedParticles(0x376A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);

                int damage = Utility.RandomMinMax(40, 60);
                // Deal damage split evenly: 50% Physical, 50% Energy
                AOS.Damage(target, this, damage, 50, 0, 0, 0, 50);

                // Drain mana and convert a portion into self-healing
                int manaDrain = Utility.RandomMinMax(15, 30);
                if (target.Mana >= manaDrain)
                {
                    target.Mana -= manaDrain;
                    target.SendMessage(0x22, "Your magical energy is sapped by the Spell Claw!");
                    target.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    target.PlaySound(0x1F8);

                    int healAmount = manaDrain / 2;
                    Hits += healAmount;
                    if (Hits > HitsMax)
                        Hits = HitsMax;
                }
            }
        }

        // Glyph of Binding – etches a magical glyph at the target location that spawns a hazardous trap (using TrapWeb)
        public void GlyphOfBinding()
        {
            if (Combatant == null || Map == null)
                return;

            Point3D targetLocation = (Combatant is Mobile target) ? target.Location : Combatant.Location;
            this.Say("*The Spell Claw etches a binding glyph!*");
            PlaySound(0x20F); // A magical glyph sound

            // Display a visual effect at the glyph location
            Effects.SendLocationParticles(EffectItem.Create(targetLocation, Map, EffectItem.DefaultDuration), 0x376A, 10, 20, UniqueHue, 0, 5039, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (Map == null)
                    return;

                Point3D spawnLoc = targetLocation;
                if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                {
                    spawnLoc.Z = Map.GetAverageZ(spawnLoc.X, spawnLoc.Y);
                    if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                        return;
                }

                TrapWeb glyph = new TrapWeb(); // Assumes TrapWeb is defined elsewhere in your shard
                glyph.Hue = UniqueHue;
                glyph.MoveToWorld(spawnLoc, Map);

                Effects.PlaySound(spawnLoc, Map, 0x1F6);
            });
        }

        // Teleport Claw – teleports behind the target and quickly strikes for extra damage.
        public void TeleportClaw(Mobile target)
        {
            if (Map == null || target == null || !target.Alive)
                return;

            // Determine an offset behind the target based on relative positions
            int offsetX = 0, offsetY = 0;
            int deltaX = X - target.X;
            int deltaY = Y - target.Y;
            if (deltaX != 0)
                offsetX = deltaX / Math.Abs(deltaX);
            if (deltaY != 0)
                offsetY = deltaY / Math.Abs(deltaY);

            Point3D newLocation = new Point3D(target.X - offsetX, target.Y - offsetY, target.Z);
            if (!Map.CanFit(newLocation.X, newLocation.Y, newLocation.Z, 16, false, false))
            {
                newLocation.Z = Map.GetAverageZ(newLocation.X, newLocation.Y);
                if (!Map.CanFit(newLocation.X, newLocation.Y, newLocation.Z, 16, false, false))
                    return;
            }

            // Teleport effects at the current and target locations
            FixedParticles(0x376A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
            PlaySound(0x1FE);

            Location = newLocation;
            ProcessDelta();

            // Shortly after teleporting, launch a swift claw strike
            Timer.DelayCall(TimeSpan.FromSeconds(0.2), () =>
            {
                if (target != null && target.Alive && CanBeHarmful(target, false))
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(30, 45);
                    AOS.Damage(target, this, damage, 50, 0, 0, 0, 50);
                    target.FixedParticles(0x376A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
                    target.SendMessage(0x22, "The Spell Claw's sudden strike sears your flesh!");
                    PlaySound(0x3E3);
                }
            });
        }

        // OnDeath – creates a dramatic arcane explosion that scatters hazardous arcane shards (using IceShardTile).
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*The Spell Claw shatters, unleashing arcane fragments!*");
                Effects.PlaySound(Location, Map, 0x20C);

                int shardsToDrop = Utility.RandomMinMax(4, 7);
                for (int i = 0; i < shardsToDrop; i++)
                {
                    int xOffset = Utility.RandomMinMax(-4, 4);
                    int yOffset = Utility.RandomMinMax(-4, 4);
                    Point3D shardLocation = new Point3D(X + xOffset, Y + yOffset, Z);

                    if (!Map.CanFit(shardLocation.X, shardLocation.Y, shardLocation.Z, 16, false, false))
                    {
                        shardLocation.Z = Map.GetAverageZ(shardLocation.X, shardLocation.Y);
                        if (!Map.CanFit(shardLocation.X, shardLocation.Y, shardLocation.Z, 16, false, false))
                            continue;
                    }

                    IceShardTile shard = new IceShardTile(); // Assumes IceShardTile is defined among your hazard tiles
                    shard.Hue = UniqueHue;
                    shard.MoveToWorld(shardLocation, Map);

                    Effects.SendLocationParticles(EffectItem.Create(shardLocation, Map, EffectItem.DefaultDuration), 0x376A, 10, 20, UniqueHue, 0, 5039, 0);
                }
            }

            base.OnDeath(c);
        }

        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 6; } }
        public override double DispelDifficulty { get { return 140.0; } }
        public override double DispelFocus { get { return 70.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 3));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));

            if (Utility.RandomDouble() < 0.02)
            {
                PackItem(new MaxxiaScroll()); // Placeholder unique magical artifact
            }
        }

        public SpellClaw(Serial serial) : base(serial)
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

            // Reinitialize ability timers
            m_NextSiphonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextGlyphTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextTeleportTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 35));
        }
    }
}
