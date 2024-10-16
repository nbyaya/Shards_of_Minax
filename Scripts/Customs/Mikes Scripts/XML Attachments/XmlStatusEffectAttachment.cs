using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.SkillHandlers;
using System.Collections.Generic;

namespace Server.Engines.XmlSpawner2
{
    public class XmlStatusEffectAttachment : XmlAttachment
    {
        // Fields for various status effects
        private int m_StrMod;
        private int m_DexMod;
        private int m_IntMod;
        private int m_HitsMod;
        private int m_StamMod;
        private int m_ManaMod;

        private SkillMod m_SkillMod;
        private ResistanceMod m_ResistMod;
        private int m_ArmorMod;

        private TimeSpan m_Duration;
        private DateTime m_EndTime;

        // Properties to allow modification via commands or scripts
        [CommandProperty(AccessLevel.GameMaster)]
        public int StrMod { get { return m_StrMod; } set { m_StrMod = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DexMod { get { return m_DexMod; } set { m_DexMod = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int IntMod { get { return m_IntMod; } set { m_IntMod = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int HitsMod { get { return m_HitsMod; } set { m_HitsMod = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int StamMod { get { return m_StamMod; } set { m_StamMod = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ManaMod { get { return m_ManaMod; } set { m_ManaMod = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ArmorMod { get { return m_ArmorMod; } set { m_ArmorMod = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Duration { get { return m_Duration; } set { m_Duration = value; } }

        public XmlStatusEffectAttachment(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlStatusEffectAttachment()
        {
            // Default values
            m_StrMod = 5;
            m_DexMod = 5;
            m_IntMod = 5;
            m_HitsMod = 10;
            m_StamMod = 10;
            m_ManaMod = 10;
            m_ArmorMod = 10;
            m_Duration = TimeSpan.FromSeconds(900);
        }

        [Attachable]
        public XmlStatusEffectAttachment(int strMod, int dexMod, int intMod, int hitsMod, int stamMod, int manaMod, int armorMod, double duration)
        {
            m_StrMod = strMod;
            m_DexMod = dexMod;
            m_IntMod = intMod;
            m_HitsMod = hitsMod;
            m_StamMod = stamMod;
            m_ManaMod = manaMod;
            m_ArmorMod = armorMod;
            m_Duration = TimeSpan.FromSeconds(duration);
        }

        public override void OnAttach()
        {
            base.OnAttach();

            Mobile mob = AttachedTo as Mobile;

            if (mob != null)
            {
                // Apply stat mods
                mob.AddStatMod(new StatMod(StatType.Str, $"XmlStrMod{Serial.Value}", m_StrMod, m_Duration));
                mob.AddStatMod(new StatMod(StatType.Dex, $"XmlDexMod{Serial.Value}", m_DexMod, m_Duration));
                mob.AddStatMod(new StatMod(StatType.Int, $"XmlIntMod{Serial.Value}", m_IntMod, m_Duration));

                // Apply hit points, stamina, mana mods
                mob.Hits += m_HitsMod;
                mob.Stam += m_StamMod;
                mob.Mana += m_ManaMod;

                // Apply skill mod (e.g., Swordsmanship)
                m_SkillMod = new DefaultSkillMod(SkillName.Swords, true, 10.0);
                mob.AddSkillMod(m_SkillMod);

                // Apply resistance mod (e.g., Physical Resistance)
                m_ResistMod = new ResistanceMod(ResistanceType.Physical, 10);
                mob.AddResistanceMod(m_ResistMod);

                // Apply armor mod
                mob.VirtualArmorMod += m_ArmorMod;

                // Start a timer to remove the attachment after the duration
                Timer.DelayCall(m_Duration, Delete);
            }
        }

        public override void OnDelete()
        {
            base.OnDelete();

            Mobile mob = AttachedTo as Mobile;

            if (mob != null)
            {
                // Remove stat mods
                mob.RemoveStatMod($"XmlStrMod{Serial.Value}");
                mob.RemoveStatMod($"XmlDexMod{Serial.Value}");
                mob.RemoveStatMod($"XmlIntMod{Serial.Value}");

                // Remove skill mod
                if (m_SkillMod != null)
                    mob.RemoveSkillMod(m_SkillMod);

                // Remove resistance mod
                if (m_ResistMod != null)
                    mob.RemoveResistanceMod(m_ResistMod);

                // Remove armor mod
                mob.VirtualArmorMod -= m_ArmorMod;
            }
        }

        public override void OnUse(Mobile from)
        {
            base.OnUse(from);

            // Example effect when the attachment is used
            from.SendMessage("You feel a surge of power!");
            // Apply some effect or trigger here
        }

        public override void OnWeaponHit(Mobile attacker, Mobile defender, BaseWeapon weapon, int damageGiven)
        {
            base.OnWeaponHit(attacker, defender, weapon, damageGiven);

            // Example effect when the attached mobile hits someone
            defender.SendMessage("You have been struck by a powerful force!");
            // Apply additional damage or effect
            defender.Damage(5, attacker);
        }

        public override void OnMovement(MovementEventArgs args)
        {
            base.OnMovement(args);

            // Example effect when the attached mobile moves
            Mobile mob = args.Mobile;
            mob.FixedParticles(0x373A, 1, 15, 9909, EffectLayer.Waist);
        }

        public override void OnSpeech(SpeechEventArgs args)
        {
            base.OnSpeech(args);

            // Example effect when the attached mobile speaks a keyword
            if (args.Speech.ToLower().Contains("power up"))
            {
                Mobile mob = args.Mobile;
                mob.SendMessage("Your powers have been enhanced!");
                // Apply temporary boost
                mob.AddStatMod(new StatMod(StatType.Str, $"XmlSpeechStrMod{Serial.Value}", 10, TimeSpan.FromSeconds(10)));
            }
        }

        public override void OnKilled(Mobile killed, Mobile killer)
        {
            base.OnKilled(killed, killer);

            // Example effect when the attached mobile is killed
            killer.SendMessage("You have defeated your foe and feel invigorated!");
            killer.Hits += 20; // Heal the killer
        }

        public override void OnKill(Mobile killed, Mobile killer)
        {
            base.OnKill(killed, killer);

            // Example effect when the attached mobile kills someone
            Mobile mob = AttachedTo as Mobile;
            if (mob == killer)
            {
                mob.SendMessage("Your victory empowers you!");
                mob.Stam += 10; // Restore stamina
            }
        }

        public override void OnTrigger(object activator, Mobile from)
        {
            base.OnTrigger(activator, from);

            // Example effect when the attachment is triggered by some event
            from.SendMessage("The status effect has been triggered!");
            // Apply or modify effects here
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            // Save fields
            writer.Write(m_StrMod);
            writer.Write(m_DexMod);
            writer.Write(m_IntMod);
            writer.Write(m_HitsMod);
            writer.Write(m_StamMod);
            writer.Write(m_ManaMod);
            writer.Write(m_ArmorMod);
            writer.Write(m_Duration);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Load fields
            m_StrMod = reader.ReadInt();
            m_DexMod = reader.ReadInt();
            m_IntMod = reader.ReadInt();
            m_HitsMod = reader.ReadInt();
            m_StamMod = reader.ReadInt();
            m_ManaMod = reader.ReadInt();
            m_ArmorMod = reader.ReadInt();
            m_Duration = reader.ReadTimeSpan();

            // Re-apply effects if necessary
        }

        public override string OnIdentify(Mobile from)
        {
            // Provide information when identified
            return $"Status Effect Attachment:\n" +
                   $"- Strength Mod: {m_StrMod}\n" +
                   $"- Dexterity Mod: {m_DexMod}\n" +
                   $"- Intelligence Mod: {m_IntMod}\n" +
                   $"- Hits Mod: {m_HitsMod}\n" +
                   $"- Stamina Mod: {m_StamMod}\n" +
                   $"- Mana Mod: {m_ManaMod}\n" +
                   $"- Armor Mod: {m_ArmorMod}\n" +
                   $"- Duration: {m_Duration.TotalSeconds} seconds";
        }
    }
}
