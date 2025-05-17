using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a reaper of rot corpse")]
    public class ReaperOfRot : BaseCreature
    {
        private DateTime _NextDecayingAura;
        private DateTime _NextRottingTouch;
        private DateTime _NextCorpseSummon;
        private int _AuraPulseCount;

        [Constructable]
        public ReaperOfRot()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a Reaper of Rot";
            this.Body = 47;
            this.BaseSoundID = 442;
            // Use a unique rotting hue. (For example, a sickly green tone.)
            this.Hue = 0x83F; 

            // Enhanced stats compared to the base Reaper.
            this.SetStr(200, 300);
            this.SetDex(100, 150);
            this.SetInt(200, 300);

            this.SetHits(300, 400);
            this.SetStam(0);
            this.SetDamage(15, 25);

            // Change damage mix: add extra poison and energy damage.
            this.SetDamageType(ResistanceType.Physical, 60);
            this.SetDamageType(ResistanceType.Poison, 30);
            this.SetDamageType(ResistanceType.Energy, 10);

            this.SetResistance(ResistanceType.Physical, 50, 65);
            this.SetResistance(ResistanceType.Fire, 25, 35);
            this.SetResistance(ResistanceType.Cold, 20, 40);
            this.SetResistance(ResistanceType.Poison, 70, 80);
            this.SetResistance(ResistanceType.Energy, 40, 50);

            // Skills boosted for magical and melee abilities.
            this.SetSkill(SkillName.EvalInt, 95.1, 110.0);
            this.SetSkill(SkillName.Magery, 95.1, 110.0);
            this.SetSkill(SkillName.MagicResist, 105.1, 130.0);
            this.SetSkill(SkillName.Tactics, 70.1, 85.0);
            this.SetSkill(SkillName.Wrestling, 70.1, 85.0);

            this.Fame = 10000;
            this.Karma = -10000;

            // Significantly tougher virtual armor.
            this.VirtualArmor = 55;

            // Loot: make it worthwhile.
            this.PackItem(new Log(20));
            this.PackItem(new MandrakeRoot(10));
            this.PackItem(new Item(Utility.RandomMinMax(0xE76, 0xE80))); // random unique item graphic code as example

            // Initialize timers for abilities.
            _NextDecayingAura = DateTime.UtcNow + TimeSpan.FromSeconds(5.0);
            _NextRottingTouch = DateTime.UtcNow + TimeSpan.FromSeconds(8.0);
            _NextCorpseSummon = DateTime.UtcNow + TimeSpan.FromSeconds(20.0);
        }

        public ReaperOfRot(Serial serial)
            : base(serial)
        {
        }

        public override Poison PoisonImmune
        {
            get { return Poison.Deadly; }
        }

        public override int TreasureMapLevel { get { return 4; } }

        // This creature does not allow movement during its rot-induced focus.
        public override bool DisallowAllMoves
        {
            get { return false; }
        }

        // Advanced AI: In addition to normal combat behavior, the Reaper of Rot uses special abilities.
        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;

            // Ensure valid target and proper range.
            if (combatant == null || combatant.Deleted || combatant.Map != Map || !InRange(combatant, 12) || !CanBeHarmful(combatant) || !InLOS(combatant))
                return;

            DateTime now = DateTime.UtcNow;

            // Ability 1: Decaying Aura
            // Every 5-7 seconds pulse an aura that damages and poisons nearby enemies.
            if (now >= _NextDecayingAura)
            {
                // Use MovingParticles effect for a visual cue.
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, TimeSpan.FromSeconds(0.5)), 
                    0x3709, 10, 30, 0x83F, 0, 5029, 0);
                PlaySound(0x223);

                // Apply poison damage over time to all mobiles within a 2-tile radius.
                foreach (Mobile m in this.GetMobilesInRange(2))
                {
                    if (m != this && CanBeHarmful(m))
                    {
                        DoHarmful(m);
                        if (m is Mobile target)
                        {
                            // Apply a custom decay effect: a chance to apply deadly poison.
                            if (Utility.RandomDouble() < 0.4)
                                target.ApplyPoison(this, Poison.Deadly);
                            // Also reduce a small amount of hit points immediately.
                            AOS.Damage(m, this, Utility.RandomMinMax(5, 10), 0, 0, 100, 0, 0);
                        }
                    }
                }
                _AuraPulseCount++;
                _NextDecayingAura = now + TimeSpan.FromSeconds(5.0 + Utility.RandomDouble() * 2.0);
            }

            // Ability 2: Rotting Touch
            // Occasionally leeches life from its primary Combatant.
            if (now >= _NextRottingTouch && Utility.RandomDouble() < 0.35)
            {
                if (combatant is Mobile target)
                {
                    DoHarmful(target);
                    target.SendAsciiMessage("The Reaper of Rot's touch saps your life!");
                    PlaySound(0x5D);

                    // Calculate damage and life steal.
                    int damage = Utility.RandomMinMax(20, 35);
                    AOS.Damage(target, this, damage, 0, 20, 0, 60, 20);

                    // Heal itself for a percentage of the damage dealt.
                    this.Hits += (damage / 3);
                    if (this.Hits > this.HitsMax) this.Hits = this.HitsMax;
                }
                _NextRottingTouch = now + TimeSpan.FromSeconds(8.0 + Utility.RandomDouble() * 4.0);
            }

            // Ability 3: Corpse Summon
            // Every so often, the Reaper of Rot animates a nearby corpse to serve it briefly as a decay spawn.
            if (now >= _NextCorpseSummon && Utility.RandomDouble() < 0.25)
            {
                // Fix conversion from IPooledEnumerable<Item> to List<Item>
                List<Item> items = new List<Item>();
                foreach (Item item in this.GetItemsInRange(5))
                    items.Add(item);

                Container corpse = null;
                foreach (Item item in items)
                {
                    // Check if the item is a Corpse and is not considered “crucial.”
                    // Adjust this check to ignore player corpses
                    Corpse c = item as Corpse;
                    if (c != null && !(c.Owner is PlayerMobile))
                    {
                        corpse = c;
                        break;
                    }
                }
                if (corpse != null)
                {
                    // Create a temporary decay spawn from the corpse.
                    Mobile spawn = new DecaySpawn();
                    spawn.MoveToWorld(corpse.Location, corpse.Map);
                    // Instead of using Map.SendMessage (which doesn't exist), broadcast using PublicOverheadMessage
                    this.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "The Reaper of Rot animates a corpse to fight for it!");
                    // Set the decay spawn to last for 20 seconds.
                    Timer.DelayCall(TimeSpan.FromSeconds(20.0), delegate { if (spawn != null && !spawn.Deleted) spawn.Delete(); });
                }
                _NextCorpseSummon = now + TimeSpan.FromSeconds(20.0 + Utility.RandomDouble() * 10.0);
            }

            base.OnActionCombat();
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            // 40% chance to unleash additional rot damage on melee attacks.
            if (Utility.RandomDouble() < 0.4)
            {
                if (defender is Mobile target)
                {
                    target.SendAsciiMessage("You feel a surge of decay as the Reaper of Rot strikes you!");
                    // Inflict extra decay damage and apply a minor slow effect.
                    AOS.Damage(target, this, Utility.RandomMinMax(15, 25), 0, 30, 0, 50, 20);
                    // Slow the target by temporarily reducing its movement speed.
                    target.Freeze(TimeSpan.FromSeconds(3.0));
                }
            }
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Rich);
            // Chance for a unique rotted relic.
            if (Utility.RandomDouble() < 0.02) // 2% chance
                this.PackItem(new RottedRelic());
        }

        public override bool CanRummageCorpses { get { return true; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(_AuraPulseCount);
            // You can write out additional state as needed.
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            _AuraPulseCount = reader.ReadInt();
        }
    }

    // A simple decay spawn created via corpse summoning.
    public class DecaySpawn : BaseCreature
    {
        [Constructable]
        public DecaySpawn()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a decay spawn";
            this.Body = 47;
            this.Hue = 0x83F;
            this.BaseSoundID = 442;

            this.SetStr(100, 120);
            this.SetDex(75, 90);
            this.SetInt(50, 70);

            this.SetHits(80, 100);

            this.SetDamage(5, 10);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 30, 40);
            this.SetResistance(ResistanceType.Cold, 20, 30);
            this.SetResistance(ResistanceType.Poison, 50, 60);

            this.Fame = 2000;
            this.Karma = -2000;

            this.VirtualArmor = 20;
        }

        public DecaySpawn(Serial serial)
            : base(serial)
        {
        }

        public override void GenerateLoot()
        {
            // Minimal loot for a summoned minion.
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

    // A placeholder for a unique item dropped by the Reaper of Rot.
    public class RottedRelic : Item
    {
        [Constructable]
        public RottedRelic() : base(0x1F14)
        {
            this.Weight = 5.0;
            this.Name = "a rotted relic";
            this.Hue = 0x83F;
        }

        public RottedRelic(Serial serial)
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
